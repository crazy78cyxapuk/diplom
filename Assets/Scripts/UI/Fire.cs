using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;

public class Fire : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private GameObject obj;

	private AudioSource music;

	private Button btn;

	private void Start()
	{
		music = gameObject.GetComponent<AudioSource>();
		btn = gameObject.GetComponent<Button>();

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
		btn.interactable = false;
		yield return new WaitForSeconds(1.5f);
		btn.interactable = true;

		music.Play();

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (btn.interactable == true)
			obj.GetComponent<PlayerController>().CreateBullet();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (btn.interactable == true)
		{
			obj.GetComponent<PlayerController>().Fire();

			StartCoroutine(Recharge());
		}
	}
}
