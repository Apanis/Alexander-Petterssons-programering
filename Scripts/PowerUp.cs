using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PowerUp : MonoBehaviour
{
    public AudioClip powerUpCollected;
    public AudioClip powerUpSet;
    public AudioClip powerUpOver;
    public AudioSource audioSource;
    private int PowerUpNumber = 0;

    public static PowerUp Instance;
    Player player;
    [HideInInspector]public GameObject playerObject;
    private PhotonView PV;
    [HideInInspector]public static bool PowerUpEnabled = true;
   
    void Start()
    {
        Instance = this;
        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.GetComponent<SphereCollider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        PV = GetComponent<PhotonView>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PhotonView>().Owner;
        playerObject = other.gameObject;
        
        PV.RPC("SetPowerUp", player);
        PV.RPC("Disabled", RpcTarget.All);
        
    }
    [PunRPC]
    public void Disabled()
    {
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<SphereCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    [PunRPC]
    public void SetPowerUp()
    {
        
        PowerUpNumber = Random.Range(1, 6);
        if(PowerUpNumber == 1)
        {
            audioSource.PlayOneShot(powerUpSet);
            playerObject.GetComponent<playerController>().SpeedUpgrade();
            Debug.Log("speed set to 18");
           
        }
        if(PowerUpNumber == 2)
        {
            audioSource.PlayOneShot(powerUpSet);
            playerObject.GetComponent<playerController>().JumpUpgrade();
            Debug.Log("jump set to 14");          
        }
        if(PowerUpNumber == 3)
        {
            audioSource.PlayOneShot(powerUpSet);
            playerObject.GetComponent<playerController>().WeaponUpgrade();
            Debug.Log("pistol speed upped.");
          
        }
        if(PowerUpNumber == 4)
        {
            audioSource.PlayOneShot(powerUpSet);
            playerObject.GetComponent<playerController>().EyeUpgrade();
            Debug.Log("Eyes boosted");
        }
        if (PowerUpNumber == 5)
        {
            audioSource.PlayOneShot(powerUpSet);
            playerObject.GetComponent<playerController>().HealthUpgrade();
            Debug.Log("healing boosted");
        }
        if (PowerUpNumber == 6)
        {
            audioSource.PlayOneShot(powerUpSet);
            playerObject.GetComponent<playerController>().WeaponUpgrade();
            Debug.Log("pistol speed upped.");
        }

        StartCoroutine(Wait());
        
        
    }
    IEnumerator Wait()
    {
        PowerUpNumber = 0;
        yield return new WaitForSeconds(15f);
        PV.RPC("RemovePowerUp", player);
        PV.RPC("Enabled", RpcTarget.All);
    }
    [PunRPC]
    public void RemovePowerUp()
    {
        playerObject.GetComponent<playerController>().ResetUpgrade();
        Debug.Log("Reset to defaults.");
    }
    [PunRPC]
    public void Enabled()
    {
        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.GetComponent<SphereCollider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        audioSource.PlayOneShot(powerUpOver);
    }
}
