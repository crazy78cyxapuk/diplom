using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    private float timer = 0;
    private Rigidbody2D rb;

    private Sprite bulletImg;

    [SerializeField] private GameObject particleSystem;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletImg = GetComponent<Sprite>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5)
        {
            Destroy(gameObject);
        }

        transform.up = rb.velocity;
    }

    private void FixedUpdate()
    {
        //Vector3 cross = Vector3.Cross(transform.forward, transform.right);
        //rb.AddTorque(VelocityValue * rb.velocity.magnitude);
        //rb.AddTorque(-rb.angularVelocity + (float)Vector3.Project(rb.angularVelocity, transform.forward));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")// || collision.gameObject.tag == "Player") ;
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(particleSystem, pos, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(particleSystem, pos, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
