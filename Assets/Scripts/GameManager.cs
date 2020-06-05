using System.Collections;
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
        NewGame();
        //AgreeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
        }
    }

    private void AgreeGame()
    {
        if (PhotonNetwork.CountOfPlayers == 2)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        positionForPlayers = GameObject.FindGameObjectWithTag("Respawn");
        Vector3 startPosition = positionForPlayers.transform.position;

        GameObject newPlayer;

        if (PhotonNetwork.IsMasterClient)
        {
            newPlayer = PhotonNetwork.Instantiate("Tank", positionForPlayers.transform.position, Quaternion.identity);
            newPlayer.tag = "Master";
            PhotonNetwork.Instantiate("UI_HP_Master", gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            positionForPlayers.transform.position = new Vector3(12.69f, 3, -4.5f);
            newPlayer = PhotonNetwork.Instantiate("Tank", positionForPlayers.transform.position, Quaternion.identity);
            newPlayer.tag = "NoMaster";
            PhotonNetwork.Instantiate("UI_HP_NoMaster", gameObject.transform.position, Quaternion.identity);
        }

        Instantiate(UI_elements);

        positionForPlayers.transform.position = startPosition;
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
