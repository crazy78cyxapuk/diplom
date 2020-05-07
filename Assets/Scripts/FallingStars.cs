using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStars : MonoBehaviour
{
    [SerializeField] private GameObject star;

    private void Start()
    {
        StartCoroutine(ShowStar());
    }

    IEnumerator ShowStar()
    {
        yield return new WaitForSeconds(3);
        CreateStar();
    }

    private void CreateStar()
    {
        //x=+-20; y=+-8
        int x = Random.Range(-20, 21);
        int y = Random.Range(-8, 9);
        Vector3 posStar = new Vector3(x, y, 108);

        Instantiate(star, posStar, Quaternion.identity);

        StartCoroutine(ShowStar());
    }

}
