using System.Collections;
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
    //[SerializeField] private GameObject bullet;
    [SerializeField] private GameObject startStvolRight;
    private bool logicVelocity;

    [SerializeField] private GameObject strengthBulletProgressBar;
    private Color color;

    public float recharge; //таймер для перезарядки


    private bool strengthOver = true;

    //дуло танка
    [SerializeField] private GameObject barrel;
    [SerializeField] private Transform aroundBarrel;
    private bool moveBarrel;
    private float speedBarrel;

    //ракета
    private float speedStrength;
    private bool createBullet;

    //UI
    [SerializeField] private Button fireBtn;

    //Photon
    private PhotonView photonView;


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

        recharge = 3f;

        color = strengthBulletProgressBar.GetComponent<SpriteRenderer>().color;

        GameObject obj = GameObject.FindGameObjectWithTag("FireBtn");
        fireBtn = obj.GetComponent<Button>();

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
    }


    void Update()
    {
        if (photonView.IsMine)
        {

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

            if (recharge <= 3f)
            {
                recharge += Time.deltaTime;
                if (fireBtn.IsInteractable() == true)
                {
                    fireBtn.interactable = false;
                }
            }
            else
            {
                if (fireBtn.IsInteractable() == false)
                {
                    fireBtn.interactable = true;
                }
            }
        }
    }

    //private void FixedUpdate()
    //{
    //    Quaternion rotation = Quaternion.FromToRotation(-transform.up, aroundObj.position - transform.position);
    //    transform.rotation = rotation * transform.rotation;
    //}

    public void Fire()
    {
        if (recharge >= 3f)
        {
            Vector3 spawnPoint;
            int speedBullet = 500 + (int)speedStrength;
            spawnPoint = startStvolRight.transform.position; //получаем координаты откуда будем стрелять
            
            GameObject pula = PhotonNetwork.Instantiate("bullet", spawnPoint, Quaternion.identity);
            Rigidbody2D rbPula = pula.GetComponent<Rigidbody2D>();


            pula.transform.right = !logicVelocity ? -startStvolRight.transform.right : startStvolRight.transform.right;
            //speedBullet = logicVelocity ? speedBullet : -speedBullet;

            rbPula.AddForce(pula.transform.right * speedBullet, ForceMode2D.Impulse); //задаем ускорение пули

            speedStrength = 0;
            createBullet = false;

            recharge = 0;

            strengthBulletProgressBar.SetActive(false);
            strengthBulletProgressBar.GetComponent<SpriteRenderer>().color = color;
        }
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
        if (recharge >= 3f)
        {
            strengthBulletProgressBar.SetActive(true);
            createBullet = true;
        }
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
}
