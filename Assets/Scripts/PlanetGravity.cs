using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    private HashSet<Rigidbody2D> affectedBodies = new HashSet<Rigidbody2D>();
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.centerOfMass = new Vector3(0, 0, 30);
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody2D body in affectedBodies)
        {
            if (body != null)
            {
                Vector2 directionToPlanet = ((Vector2)transform.position - body.position).normalized;

                float distance = ((Vector2)transform.position - body.position).magnitude;
                float strength = 10 * body.mass * rb.mass / (distance * distance);

                body.AddForce(directionToPlanet * strength);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody != null)
        {
            affectedBodies.Add(collision.attachedRigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            affectedBodies.Remove(collision.attachedRigidbody);
        }
    }
}
