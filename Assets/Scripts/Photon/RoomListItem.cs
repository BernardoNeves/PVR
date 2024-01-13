using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    RoomInfo roomInfo;

    public void SetUp(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        roomName.text = _roomInfo.Name;
    }

    public void Join()
    {
        Launcher.Instance.JoinRoom(roomInfo);
    }
}
