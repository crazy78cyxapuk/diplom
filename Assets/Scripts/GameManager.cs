﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{
    private GameObject positionForPlayers;
    [SerializeField] private GameObject UI_elements;

    private void Start()
    {
        positionForPlayers = GameObject.FindGameObjectWithTag("Respawn");
        
        //Vector3 pos = positionForPlayers.transform.position;

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject newPlayer = PhotonNetwork.Instantiate("newTank", positionForPlayers.transform.position, Quaternion.identity);
            newPlayer.tag = "Master";
        }
        else
        {
            positionForPlayers.transform.position = new Vector3(12, 3, -4.5f);
            GameObject newPlayer = PhotonNetwork.Instantiate("newTank", positionForPlayers.transform.position, Quaternion.identity);
            newPlayer.tag = "NoMaster";
        }

        //GameObject newPlayer = PhotonNetwork.Instantiate("newTank", pos, Quaternion.identity);
        //newPlayer.tag = GetComponent<PhotonView>().ViewID.ToString();
        //positionForPlayers.transform.position = new Vector3(12, 3, -4.5f);

        Instantiate(UI_elements);

        //fire.onClick.AddListener(() => newPlayer.GetComponent<PlayerController>().CreateBullet());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        //когда текущйи игрок ливает с комнаты
        SceneManager.LoadScene("Menu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //когда игрок вошел в комнату
        Debug.Log(newPlayer.NickName + "    connected on room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //когда игрок покидает комнату
        Debug.Log(otherPlayer.NickName + "    left room");
    }
}