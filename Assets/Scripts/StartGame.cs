using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playersReady;

    private void Start()
    {
        playersReady = 0;
    }

    private void Update()
    {
        if (playersReady == 2)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("GameManager");

            PhotonView pv = obj.GetComponent<PhotonView>();
            pv.RPC("Play", RpcTarget.All);



            pv = gameObject.GetComponent<PhotonView>();
            if (pv.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void PlusPlayerReady()
    {
        playersReady++;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playersReady);
        }
        else
        {
            playersReady = (int)stream.ReceiveNext();
        }
    }
}
