using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private float timer = 0;
    private Rigidbody2D rb;
    private bool destr;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        destr = false;
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
        if (collision.gameObject.tag == "Planet")
        {
            if (destr == false)
            {
                destr = true;
                DestroyBullet();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NoMaster" || collision.gameObject.tag == "Master" || collision.gameObject.tag == "Bullet")
        {
            if (destr == false)
            {
                destr = true;
                DestroyBullet();
            }
        }
    }
}
