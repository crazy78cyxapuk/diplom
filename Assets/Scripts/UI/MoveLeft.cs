using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveLeft : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private GameObject obj;

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			obj = GameObject.FindGameObjectWithTag("Master");
		}
		else
		{
			obj = GameObject.FindGameObjectWithTag("NoMaster");
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		obj.GetComponent<PlayerController>().MoveLeft();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		obj.GetComponent<PlayerController>().StopMove();
	}
}
