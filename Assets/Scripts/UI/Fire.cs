using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Photon.Pun;

public class Fire : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
		obj.GetComponent<PlayerController>().CreateBullet();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		obj.GetComponent<PlayerController>().Fire();
	}
}
