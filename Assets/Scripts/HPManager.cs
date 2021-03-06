﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class HPManager : MonoBehaviourPunCallbacks
{
    private int hpBotTank;

    private PhotonView pv;

    private void Start()
    {
        hpBotTank = 80;

        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void Damage()
    {
        if (pv.IsMine)
        {
            int minusHP = Random.Range(0, 6) + 20;
            hpBotTank -= minusHP;

            if (hpBotTank < 1)
            {
                DestroyTankAndShowScreenReload();
            }

            GameObject obj;

            if (PhotonNetwork.IsMasterClient)
            {
                obj = GameObject.FindGameObjectWithTag("MasterHP");
                PhotonView pv = obj.GetComponent<PhotonView>();
                pv.RPC("ShowBar", RpcTarget.All, minusHP);
            }
            else
            {
                obj = GameObject.FindGameObjectWithTag("NoMasterHP");
                PhotonView pv = obj.GetComponent<PhotonView>();
                pv.RPC("ShowBar", RpcTarget.All, minusHP);
            }
        }
    }

    private void DestroyTankAndShowScreenReload()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        obj.GetComponent<GameManager>().GameOver();
        PhotonView pv = obj.GetComponent<PhotonView>();
        pv.RPC("GameOver", RpcTarget.All);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            
            pv.RPC("Damage", RpcTarget.All);

            Debug.Log("HPManager!!!!!!");
        }
    }
}
