using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class roomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public RoomInfo info;
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name.ToUpper();
    }
    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
