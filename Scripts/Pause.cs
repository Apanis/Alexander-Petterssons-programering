using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviourPunCallbacks
{
    public static bool paused = false;
    public static bool disconnecting = false;
    
    public void TogglePause()
    {
        if (disconnecting) return;

        paused = !paused;

        transform.GetChild(0).gameObject.SetActive(paused);
        transform.GetChild(1).gameObject.SetActive(paused);
        playerController.instance.Crosshair.enabled = !paused;
        Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = paused;
    }
    
    public void Leave()
    {
        disconnecting = true;
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }
}
