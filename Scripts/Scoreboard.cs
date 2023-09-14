using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    private TMP_Text mapName;

    void Start()
    {
        mapName = transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetComponent<TMP_Text>();
        mapName.text = SceneManager.GetActiveScene().name.ToString();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
