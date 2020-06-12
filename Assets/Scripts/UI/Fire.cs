using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;
using Photon.Pun;
using UnityEngine.UI;

public class Fire : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private GameObject obj;

	private AudioSource music;

	private void Start()
	{
		music = gameObject.GetComponent<AudioSource>();

		if (PhotonNetwork.IsMasterClient)
		{
			obj = GameObject.FindGameObjectWithTag("Master");
		}
		else
		{
			obj = GameObject.FindGameObjectWithTag("NoMaster");
		}
	}

	IEnumerator Recharge()
	{
		Button btn = gameObject.GetComponent<Button>();

		btn.interactable = false;
		yield return new WaitForSeconds(3);
		btn.interactable = true;

		music.Play();

	}

	public void OnPointerEnter(PointerEventData eventData)
	{		
		obj.GetComponent<PlayerController>().CreateBullet();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		obj.GetComponent<PlayerController>().Fire();

		StartCoroutine(Recharge());
	}
}
