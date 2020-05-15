using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    private int hpBotTank;
    private const float HP_TANK = 250;
    [SerializeField] private Text statusTxt;

    [SerializeField] private Image hpRepeat, hpStatus;

    private void Start()
    {
        hpBotTank = 250;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            int minusHP = Random.Range(0, 6) + 20;
            hpBotTank -= minusHP;
            StartCoroutine(TakeAwayHP(minusHP));

            if (hpBotTank < 1)
            {
                StartCoroutine(ReloadGame());
            }
        }
    }

    IEnumerator ReloadGame()
    {
        statusTxt.text = "Reload Game 3...";
        yield return new WaitForSeconds(1);
        statusTxt.text = "Reload Game 2...";
        yield return new WaitForSeconds(1);
        statusTxt.text = "Reload Game 1...";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

    IEnumerator TakeAwayHP(int t) //отнимаем здоровье
    {
        hpStatus.fillAmount -= t / HP_TANK;
        yield return new WaitForSeconds(1);
        hpRepeat.fillAmount = hpStatus.fillAmount;
    }
}
