using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadyPlayer : MonoBehaviourPunCallbacks
{
    private Button button;
    PhotonView pv;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        pv = gameObject.GetComponent<PhotonView>();
    }

    public void AddReadyPlayer()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("ReadyPlayers");
        PhotonView photonView = obj.GetComponent<PhotonView>();
        photonView.RPC("PlusPlayerReady", RpcTarget.All);
    }

    public void OnClick()
    {
        AddReadyPlayer();
        button.interactable = false;
    }

    public void LeaveRoom()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        obj.GetComponent<GameManager>().LeaveRoom();
    }
}
