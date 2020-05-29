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
    private const float HP_TANK = 250;

    private Image hpRepeat, hpStatus;

    private PhotonView pv;

    private void Start()
    {
        hpBotTank = 250;

        pv = GetComponent<PhotonView>();

        InitHPObjects();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            int minusHP = Random.Range(0, 6) + 20;
            hpBotTank -= minusHP;
            StartCoroutine(TakeAwayHP(minusHP));

            if (hpBotTank < 1)
            {
                LeaveRoom();
            }
        }
    }

    private void InitHPObjects()
    {
        GameObject obj;

        if (PhotonNetwork.IsMasterClient)
        {
            obj = GameObject.FindGameObjectWithTag("MasterHP");

        }
        else
        {
            obj = GameObject.FindGameObjectWithTag("NoMasterHP");
        }

        hpRepeat = obj.transform.GetChild(0).GetComponent<Image>();
        hpStatus = obj.transform.GetChild(1).GetComponent<Image>();
    }

    IEnumerator TakeAwayHP(int t) //отнимаем здоровье
    {
        if (pv.IsMine)
        {
            hpStatus.fillAmount -= t / HP_TANK;
            yield return new WaitForSeconds(1);
            hpRepeat.fillAmount = hpStatus.fillAmount;
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
