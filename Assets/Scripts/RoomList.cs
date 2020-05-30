using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    [SerializeField] private Text txt;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        txt.text = roomInfo.MaxPlayers + ", " + roomInfo.Name;
    }
}
