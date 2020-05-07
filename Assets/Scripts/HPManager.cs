using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    private int hpBotTank;
    public Text hpBotTankTxt;

    private void Start()
    {
        hpBotTank = 500;
        hpBotTankTxt.text = hpBotTank.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hpBotTank -= Random.Range(0, 6) + 10;
            if (hpBotTank < 1)
            {
                StartCoroutine(ReloadGame());
            }
            else
            {
                hpBotTankTxt.text = hpBotTank.ToString();
            }
        }
    }

    IEnumerator ReloadGame()
    {
        hpBotTankTxt.text = "Reload Game 3...";
        yield return new WaitForSeconds(1);
        hpBotTankTxt.text = "Reload Game 2...";
        yield return new WaitForSeconds(1);
        hpBotTankTxt.text = "Reload Game 1...";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}
