using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;


public class playerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    GameObject controller;
    public static bool isAlive;
    
    
    void Awake()
    {
        isAlive = true;
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if(PV.IsMine)
        {
            CreateController();
        }
    }
    

    
    
    void CreateController()
    {
        isAlive = true;
        Transform spawnpoint = spawnManager.Instance.GetSpawnPoint();

        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerPrefs.GetString("Skin")), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
       

        
    }
    public void Die()
    {
        isAlive = false;
        PhotonNetwork.Destroy(controller);
        Invoke("RespawnCooldown", 3f);
    }
    private void RespawnCooldown()
    {
        isAlive = true;
        CreateController();
    }
}
