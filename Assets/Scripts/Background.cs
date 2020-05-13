using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject background;

    private bool create = false;

    void Update()
    {
        transform.position -= new Vector3(.05f, 0, 0);


        if (create == false && transform.position.x < 25)
        {
            Vector3 position = new Vector3(84, 0, 106.5f);
            Instantiate(background, position, Quaternion.identity);
            create = true;
        }

        if (transform.position.x < -70)
        {
            Destroy(gameObject);
        }
    }
}
