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
    private GameObject clone_UI_elements;

    private PhotonView pv;

    private GameObject UI_AgreeGame;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("readyPlayers", gameObject.transform.position, Quaternion.identity);
            UI_AgreeGame = PhotonNetwork.Instantiate("ReloadGameUI", gameObject.transform.position, Quaternion.identity);
        }

        pv = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveRoom();
        }
    }

    [PunRPC]
    public void Play()
    {
        //ClearField();
        pv.RPC("ClearField", RpcTarget.All);

        NewGame();
    }

    [PunRPC]
    private void ClearField()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Master");
            if (obj != null)
                PhotonNetwork.Destroy(obj);

            obj = GameObject.FindGameObjectWithTag("UI_HP_Master");
            if (obj != null)
                PhotonNetwork.Destroy(obj);
        }
        else
        {
            GameObject obj = GameObject.FindGameObjectWithTag("NoMaster");
            if (obj != null)
                PhotonNetwork.Destroy(obj);

            obj = GameObject.FindGameObjectWithTag("UI_HP_NoMaster");
            if (obj != null)
                PhotonNetwork.Destroy(obj);
        }

        if (clone_UI_elements != null)
            Destroy(clone_UI_elements);
    }

    [PunRPC]
    public void GameOver()
    {
        ClearField();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("readyPlayers", gameObject.transform.position, Quaternion.identity);
            UI_AgreeGame = PhotonNetwork.Instantiate("ReloadGameUI", gameObject.transform.position, Quaternion.identity);
        }
    }

    private void NewGame()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(UI_AgreeGame);

        positionForPlayers = GameObject.FindGameObjectWithTag("Respawn");
        Vector3 startPosition = positionForPlayers.transform.position;

        clone_UI_elements = Instantiate(UI_elements);

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

        pv.RPC("GameOver", RpcTarget.All);
    }
}
