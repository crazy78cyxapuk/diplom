using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //танк
    [SerializeField] private Transform aroundObj;
    [SerializeField] private float rotSpeed;
    private float speed;
    private bool goMove;
    private bool flipRight;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject startStvolRight;
    private bool logicVelocity;

    //дуло танка
    [SerializeField] private GameObject barrel;
    [SerializeField] private Transform aroundBarrel;
    private bool moveBarrel;
    private float speedBarrel;

    //ракета
    private int speedStrength;
    private bool createBullet;

    private void Start()
    {
        goMove = false;
        flipRight = true;
        logicVelocity = true;
        createBullet = false;

        speedBarrel = 0.2f;
        moveBarrel = false;

        speedStrength = 0;
    }


    void Update()
    {

#if UNITY_EDITOR
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

        if (Input.GetKey(KeyCode.Space))
        {
            ControlSpeedBullet();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Fire();
        }
#endif


        if (goMove)
        {
            transform.RotateAround(aroundObj.position, new Vector3(0, 0, -1), speed);
        }

        if (moveBarrel)
        {
            if (speedBarrel < 0 && barrel.transform.rotation.eulerAngles.z > -15)
            {
                barrel.transform.RotateAround(aroundBarrel.position, new Vector3(0, 0, -1), -1f);//speedBarrel);
                
                
                Debug.Log(TranslateEulerToRotate(barrel.transform.rotation.z) + "rotation");

            }

            //if (speedBarrel > 0 && barrel.transform.localEulerAngles.z > 15)
            //{
            //    barrel.transform.RotateAround(aroundBarrel.position, new Vector3(0, 0, -1), speedBarrel);
            //}
        }

        if (createBullet)
        {
            ControlSpeedBullet();
        }
        
    }

    private void FixedUpdate()
    {
        //поворачиваем игрока всегда в сторону планеты, на которой он стоит
        Quaternion rotation = Quaternion.FromToRotation(-transform.up, aroundObj.position - transform.position);
        transform.rotation = rotation * transform.rotation;
    }

    public void Fire()
    {
        Vector3 spawnPoint;
        int speedBullet = 500 + speedStrength;
        spawnPoint = startStvolRight.transform.position; //получаем координаты откуда будем стрелять
        GameObject pula = Instantiate(bullet, spawnPoint, Quaternion.identity);//Quaternion.Euler(0, 0, 50+startStvolRight.transform.rotation.z));  //Quaternion.Euler(0f, 0f, -90f)); //create bullet      quaternionStvol);// *
        Rigidbody2D rbPula = pula.GetComponent<Rigidbody2D>();

        
        pula.transform.right = !logicVelocity ? -startStvolRight.transform.right : startStvolRight.transform.right;
        speedBullet = !logicVelocity ? speedBullet : -speedBullet;

        rbPula.AddForce(pula.transform.right * speedBullet, ForceMode2D.Impulse); //задаем ускорение пули

        speedStrength = 0;
        createBullet = false;
    }

    public void ControlSpeedBullet()
    {
        if (speedStrength < 300)
        {
            speedStrength += 10;
        }
        else
        {
            Fire();
        }
    }

    public void CreateBullet()
    {
        createBullet = true;
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
    
    public void MoveBarrelUp()
    {
        moveBarrel = true;

        speedBarrel = - 0.2f;
    }

    public void MoveBarrelDown()
    {
        moveBarrel = true;

        speedBarrel = 0.2f;
    }

    public void MoveBarrelStop()
    {
        moveBarrel = false;
    }

    public float TranslateEulerToRotate(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }
}
