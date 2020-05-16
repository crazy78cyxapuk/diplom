using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Кинуть скрипт до сцены с игрой, то есть на меню
    void Start()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(1000, 10000);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        Debug.Log("connect");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("room join");

        PhotonNetwork.LoadLevel("Game"); //сцена с игрой
    }
}
