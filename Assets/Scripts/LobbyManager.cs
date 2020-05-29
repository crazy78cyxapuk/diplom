using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button[] btn;

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
        Debug.Log("connect");

        for(int i=0; i<btn.Length; i++)
        {
            btn[i].interactable = true;
        }
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
