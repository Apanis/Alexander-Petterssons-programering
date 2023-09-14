using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AutomaticGun : Gun
{
    [SerializeField] Camera cam;
    PhotonView PV;
    private float cooldown = 0.08f;
    float lastShot;
    public Image hitmarkerImage;
    private float hitmarkerWait;
    public ParticleSystem muzzleFlash;
    [Header("Reference Points")]
    public Transform recoilPosition;
    public Transform rotationPoint;
    [Space(10)]
    [Header("Speed Settings")]
    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    [Space(10)]
    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 38f;
    [Space(10)]
    [Header("Amount Settings")]
    public Vector3 RecoilRotation = new Vector3(10, 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);
    [Space(10)]
   
    Vector3 rotationalRecoil;
    Vector3 positionalRecoil;
    Vector3 rot;
    public CameraShake cameraShake;
    public AudioClip lazerShot;
    public AudioClip hitmarkerSound;
    public AudioClip HeadShotSound;
    public AudioSource audioSource;
    public static AutomaticGun Instance;


    


    void Awake()
    {
        //cooldown = PlayerPrefs.GetFloat("AKCOOLDOWN");
        Instance = this;
        PV = GetComponent<PhotonView>();
        hitmarkerImage.color = new Color(1, 1, 1, 0);
    }
    public override void Use()
    {
        Shoot();
    }
    void Shoot()
    {   
        if(Time.time-lastShot<cooldown)
            return;
        lastShot = Time.time;
        StartCoroutine(cameraShake.Shake(0.1f, .005f));
        rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        positionalRecoil += new Vector3(Random.Range(-RecoilKickBack.x, RecoilKickBack.x), Random.Range(-RecoilKickBack.y, RecoilKickBack.y), RecoilKickBack.z);
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {   
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            if(hit.collider.gameObject.tag == "Player")
            {
                

                
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((gunInfo)itemInfo).damage);

                audioSource.PlayOneShot(hitmarkerSound);
                hitmarkerImage.color = Color.white;
                hitmarkerWait = 0.25f;
                
            }
            if (hit.collider.gameObject.tag == "PlayerHead")
            {
                hit.collider.transform.parent.gameObject.GetComponent<IDamageable>()?.TakeDamage(((gunInfo)itemInfo).damage * 2);

                

                
                audioSource.PlayOneShot(HeadShotSound);
                hitmarkerImage.color = Color.white;
                hitmarkerWait = 0.25f;
                
            }

        }
        
    }
    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(lazerShot);
        muzzleFlash.Play();
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
            
                
            
        }
    }
    void Update()
    {
        if(hitmarkerWait > 0)
        {
            hitmarkerWait -= Time.deltaTime;
        }
        else
        {
            hitmarkerImage.color = Color.Lerp(hitmarkerImage.color, new Color(1, 1, 1, 0), Time.deltaTime * 3.5f);
        }
    }
    void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        rot = Vector3.Slerp(rot, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        rotationPoint.localRotation = Quaternion.Euler(rot);
    }
}
