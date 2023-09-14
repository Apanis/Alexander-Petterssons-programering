using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

public class Kick : MonoBehaviourPunCallbacks
{
    Player player;
    void Start()
    {
        
    }
    public void OnClick(Player otherPlayer)
    {
        if(player.IsMasterClient == true)
        {
            
        }
    }
}
