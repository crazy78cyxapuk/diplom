using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform aroundObj;
    public float rotSpeed = 0f;
    private float speed;
    private bool goMove;
    private bool flipRight;
    //public Transform rig;
    public GameObject bullet;
    public GameObject startStvolRight;
    private bool logicVelocity = true;

    private void Start()
    {
        goMove = false;
        flipRight = true;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopMove();
        }



        if (goMove)
        {
            transform.RotateAround(aroundObj.position, new Vector3(0, 0, -1), speed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fire();
        }
    }

    private void FixedUpdate()
    {
        //поворачиваем игрока всегда в сторону планеты, на которой он стоит
        Quaternion rotation = Quaternion.FromToRotation(-transform.up, aroundObj.position - transform.position);
        transform.rotation = rotation * transform.rotation;
    }

    public void fire()
    {
        Vector3 spawnPoint;
        Quaternion quaternionStvol;
        int speedBullet = 500;
        spawnPoint = startStvolRight.transform.position; //получаем координаты откуда будем стрелять
        GameObject pula = Instantiate(bullet, spawnPoint, Quaternion.Euler(0, 0, 50+startStvolRight.transform.rotation.z));  //Quaternion.Euler(0f, 0f, -90f)); //create bullet      quaternionStvol);// *
        Rigidbody2D rbPula = pula.GetComponent<Rigidbody2D>();

        
        pula.transform.right = !logicVelocity ? -startStvolRight.transform.right : startStvolRight.transform.right;
        speedBullet = !logicVelocity ? 500 : -500;
        rbPula.AddForce(transform.right * speedBullet, ForceMode2D.Impulse); //задаем ускорение пули
    }

    public void StopMove()
    {
        goMove = false;
    }

    public void MoveRight()
    {
        logicVelocity = true;
        goMove = true;

        if (!flipRight)
        {
            flipRight = true;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }

        gameObject.transform.GetComponent<SpriteRenderer>().flipX = false;
        speed = rotSpeed;
    }

    public void MoveLeft()
    {
        logicVelocity = false;
        goMove = true;

        if (flipRight)
        {
            flipRight = false;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }

        speed = -rotSpeed;
    }
}
