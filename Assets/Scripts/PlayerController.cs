﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    //танк
    private Transform aroundObj;
    [SerializeField] private float rotSpeed;
    private float speed;
    private bool goMove;
    private bool flipRight;
    [SerializeField] private GameObject startStvolRight;
    private bool logicVelocity;

    [SerializeField] private GameObject strengthBulletProgressBar;
    private Color color;


    private bool strengthOver = true;

    //дуло танка
    [SerializeField] private GameObject barrel;
    [SerializeField] private Transform aroundBarrel;
    private bool moveBarrel;
    private float speedBarrel;

    //ракета
    private float speedStrength;
    private bool createBullet;

    //Photon
    private PhotonView photonView;

    //Music
    private AudioSource music;
    [SerializeField] private AudioClip engine;
    private bool _playMusic;


    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        goMove = false;
        flipRight = false;
        logicVelocity = false;
        createBullet = false;

        speedBarrel = 0.2f;
        moveBarrel = false;

        speedStrength = 0;

        color = strengthBulletProgressBar.GetComponent<SpriteRenderer>().color;

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject around = GameObject.Find("orangePlanet");
            aroundObj = around.transform;
        }
        else
        {
            GameObject around = GameObject.Find("bluePlanet");
            aroundObj = around.transform;
        }

        InitMusic();
    }


    void Update()
    {
        if (photonView.IsMine)
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
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                StopMove();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                CreateBullet();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Fire();
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveBarrelUp();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveBarrelDown();
            }
            if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                MoveBarrelStop();
            }
#endif


            if (goMove)
            {
                transform.RotateAround(aroundObj.position, new Vector3(0, 0, -1), speed);
            }

            if (moveBarrel)
            {
                if (TranslateEulerToRotate(barrel.transform.localRotation.eulerAngles.z) >= -15 && TranslateEulerToRotate(barrel.transform.localRotation.eulerAngles.z) <= 15)
                {
                    barrel.transform.RotateAround(aroundBarrel.position, new Vector3(0, 0, -1), speedBarrel);
                }
            }

            if (createBullet)
            {
                ControlSpeedBullet();
            }
        }
    }

    public void Fire()
    {
        Vector3 spawnPoint;
        int speedBullet = 500 + (int)speedStrength;
        spawnPoint = startStvolRight.transform.position;

        GameObject pula = PhotonNetwork.Instantiate("bullet", spawnPoint, Quaternion.identity);
        Rigidbody2D rbPula = pula.GetComponent<Rigidbody2D>();


        pula.transform.right = !logicVelocity ? -startStvolRight.transform.right : startStvolRight.transform.right;

        rbPula.AddForce(pula.transform.right * speedBullet, ForceMode2D.Impulse);

        speedStrength = 0;
        createBullet = false;

        strengthBulletProgressBar.SetActive(false);
        strengthBulletProgressBar.GetComponent<SpriteRenderer>().color = color;
    }

    public void ControlSpeedBullet()
    {
        if (strengthOver)
        {
            if (speedStrength < 300)
            {
                speedStrength += 3;
                strengthBulletProgressBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, speedStrength/300);
            }
            else
            {
                strengthOver = false;
            }
        }
        else
        {
            if (speedStrength > 0)
            {
                speedStrength -= 3;
                strengthBulletProgressBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, (1-speedStrength/300));
            }
            else
            {
                strengthOver = true;
            }
        }
    }

    public void CreateBullet()
    {
        strengthBulletProgressBar.SetActive(true);
        createBullet = true;
    }

    public void StopMove()
    {
        goMove = false;
        music.volume = 0.2f;
        _playMusic = true;
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

        speed = rotSpeed;

        if (_playMusic == true)
        {
            music.volume = 0.8f;
            _playMusic = false;
        }
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

        if (_playMusic == true)
        {
            music.volume = 0.8f;
            _playMusic = false;
        }
    }
    
    public void MoveBarrelUp()
    {
        if (TranslateEulerToRotate(barrel.transform.localRotation.eulerAngles.z) > 15)
        {
            barrel.transform.RotateAround(aroundBarrel.position, new Vector3(0, 0, -1), logicVelocity ? -0.2f : 0.2f);
        }


        moveBarrel = true;
        speedBarrel = logicVelocity ? -0.2f : 0.2f;
    }

    public void MoveBarrelDown()
    {
        if (TranslateEulerToRotate(barrel.transform.localRotation.eulerAngles.z) < -15)
        {
            barrel.transform.RotateAround(aroundBarrel.position, new Vector3(0, 0, -1), logicVelocity ? 0.2f : -0.2f);
        }

        moveBarrel = true;
        speedBarrel = logicVelocity ? 0.2f : -0.2f;
    }

    public void MoveBarrelStop()
    {
        moveBarrel = false;
    }

    public float TranslateEulerToRotate(float x)
    {
        if (x >= -90 && x <= 90)
            return x;

        x = x % 180;
        
        if (x > 0)
            x -= 180;
        else
            x += 180;
        
        return x;
    }

    private void InitMusic()
    {
        music = gameObject.GetComponent<AudioSource>();
        music.clip = engine;
        music.volume = 0.2f;
        music.Play();
    }
}
