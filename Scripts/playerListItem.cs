using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class playerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text text;
    public void SetUp(Player _player)
    {
        player = _player;
        if(_player.IsMasterClient)
        {
            text.color = Color.yellow;
            text.text = _player.NickName + "(Leader)";
        }
        if(_player.IsMasterClient == false)
        {
            text.color = Color.green;
            text.text = _player.NickName;
        } 
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(newMasterClient.IsMasterClient)
        {
            text.color = Color.yellow;
            text.text = newMasterClient.NickName + "(Leader)";
        }
        if(newMasterClient.IsMasterClient == false)
        {
            text.color = Color.green;
            text.text = newMasterClient.NickName;
        } 
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
        
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
        
    }
}
