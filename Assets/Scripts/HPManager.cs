using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class HPManager : MonoBehaviourPun, IPunObservable//MonoBehaviourPunCallbacks,
{
    private int hpBotTank;
    private const float HP_TANK = 250;

    //private Image hpRepeat, hpStatus;
    private Image MasterHPRepeat, MasterHPStatus, NoMasterHPRepeat, NoMasterHPStatus;

    private PhotonView pv;

    private void Start()
    {
        hpBotTank = 250;

        pv = GetComponent<PhotonView>();

        InitHPObjects();
    }

    private void InitHPObjects()
    {
        GameObject obj;

        obj = GameObject.FindGameObjectWithTag("MasterHP");
        MasterHPRepeat = obj.transform.GetChild(0).GetComponent<Image>();
        MasterHPStatus = obj.transform.GetChild(1).GetComponent<Image>();

        obj = GameObject.FindGameObjectWithTag("NoMasterHP");
        NoMasterHPRepeat = obj.transform.GetChild(0).GetComponent<Image>();
        NoMasterHPStatus = obj.transform.GetChild(1).GetComponent<Image>();
    }

    IEnumerator TakeAwayHP(Image hpStatus, Image hpRepeat) //отнимаем здоровье
    {
        if (pv.IsMine)
        {
            int minusHP = Random.Range(0, 6) + 20;
            hpBotTank -= minusHP;

            if (hpBotTank < 1)
            {
                LeaveRoom();
            }
            else
            {
                hpStatus.fillAmount -= minusHP / HP_TANK;
                yield return new WaitForSeconds(1);
                hpRepeat.fillAmount = hpStatus.fillAmount;
            }
        }
    }

    public void MasterTakeAwayHP()
    {
        StartCoroutine(TakeAwayHP(MasterHPStatus, MasterHPRepeat));
    }

    public void NoMasterTakeAwayHP()
    {
        StartCoroutine(TakeAwayHP(NoMasterHPStatus, NoMasterHPRepeat));
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(NoMasterHPRepeat.fillAmount);
            stream.SendNext(NoMasterHPStatus.fillAmount);
            stream.SendNext(MasterHPRepeat.fillAmount);
            stream.SendNext(MasterHPStatus.fillAmount);
        }
        else
        {
            NoMasterHPRepeat.fillAmount = (float)stream.ReceiveNext();
            NoMasterHPStatus.fillAmount = (float)stream.ReceiveNext();
            MasterHPRepeat.fillAmount = (float)stream.ReceiveNext();
            MasterHPStatus.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
