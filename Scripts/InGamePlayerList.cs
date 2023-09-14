using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class InGamePlayerList : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text text;
    private GameObject ScoreboardObject;

    private void Awake()
    {
        ScoreboardObject = GameObject.Find("Scoreboard").transform.GetChild(0).transform.GetChild(0).gameObject;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (player == newPlayer)
        {
            Instantiate(gameObject, ScoreboardObject.transform);
            text.text = newPlayer.NickName;
        }

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }

    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
