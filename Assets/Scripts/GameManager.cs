using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject positionForPlayers;

    private void Start()
    {
        Vector3 pos = positionForPlayers.transform.position;

        PhotonNetwork.Instantiate(PlayerPrefab.name, pos, Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        //когда текущйи игрок ливает с комнаты
        SceneManager.LoadScene("Menu");
    }

    public void LeaveRoom()
    {
        SceneManager.LoadScene("Menu");
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
