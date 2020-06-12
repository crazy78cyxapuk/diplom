using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    private float timer = 0;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5)
        {
            DestroyBullet();
        }

        transform.up = rb.velocity;
    }

    [PunRPC]
    private void DestroyBullet()
    {
        PhotonView pv = gameObject.GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            PhotonNetwork.Instantiate("BoomPS", pos, Quaternion.identity);

            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "NoMaster" || collision.gameObject.tag == "Master" || collision.gameObject.tag == "Bullet")
        {
            //PhotonView pv = gameObject.GetComponent<PhotonView>();
            //pv.RPC("DestroyBullet", RpcTarget.All);

            DestroyBullet();
        }
    }
}
