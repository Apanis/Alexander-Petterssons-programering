using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Knife : Gun 
{
    [SerializeField] Camera cam;
    PhotonView PV;
    private float cooldown = 0.8f;
    float lastShot;
    public Image hitmarkerImage;
    private float hitmarkerWait;
    public CameraShake cameraShake;
    public AudioClip swing;
    public AudioClip KnifeHit;
    public AudioSource audioSource;
    private Animator anim;

    
    
        

    void Awake()
    {
        anim = GetComponent<Animator>();
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
        StartCoroutine(cameraShake.Shake(0.2f, .01f));
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        PV.RPC("RPC_Effects", RpcTarget.All);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {   

            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            if (hit.collider.gameObject.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((gunInfo)itemInfo).damage);
                audioSource.PlayOneShot(KnifeHit);
                hitmarkerImage.color = Color.white;
                hitmarkerWait = 0.25f;
            }
            if (hit.collider.gameObject.tag == "PlayerHead")
            {
                hit.collider.transform.parent.gameObject.GetComponent<IDamageable>()?.TakeDamage(((gunInfo)itemInfo).damage * 2);
                audioSource.PlayOneShot(KnifeHit);
                hitmarkerImage.color = Color.white;
                hitmarkerWait = 0.25f;
            }
        }
        
    }
    
    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    { 
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }

    [PunRPC]
    void RPC_Effects()
    { 
        anim.Play("KnifeSwing1");
        audioSource.pitch = Random.Range(0.9f, 1.4f);
        audioSource.PlayOneShot(swing);
        anim.Play("KnifeSwing2");
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

        if(Input.GetKeyDown(KeyCode.F))
        {
            //anim.Play("KnifeInspect1");
        }
    }
}
