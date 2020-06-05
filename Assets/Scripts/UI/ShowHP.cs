using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShowHP : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Image HPRepeat, HPStatus;

    private float HP_TANK = 250;

    [PunRPC]
    public void ShowBar(int minusHP)
    { 
        StartCoroutine(ShowTakeAway(minusHP));
    }

    [PunRPC]
    IEnumerator ShowTakeAway(int minusHP)
    {
        PhotonView pv = gameObject.GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            HPStatus.fillAmount -= (float)minusHP / HP_TANK;
            yield return new WaitForSeconds(1);
            HPRepeat.fillAmount = HPStatus.fillAmount;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HPRepeat.fillAmount);
            stream.SendNext(HPStatus.fillAmount);
        }
        else
        {
            HPRepeat.fillAmount = (float)stream.ReceiveNext();
            HPStatus.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
