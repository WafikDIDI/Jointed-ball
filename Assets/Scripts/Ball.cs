using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball Instance;
    
    private float speed;

    private Rigidbody rigidbodyReference;
    public Rigidbody RigidbodyReference => rigidbodyReference;

    private Vector3 lastFrameVelocity;

    private void Awake ()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(this);
            }
        }
        else
        {
            Instance = this;
        }
    }

    private void Start ()
    {
        rigidbodyReference = GetComponent<Rigidbody>();
    }

    private void Update ()
    {
        lastFrameVelocity = rigidbodyReference.velocity;
    }

    private void FixedUpdate ()
    {
        if ((speed > 0) && (Player.Instance.IsKickingTheBall == false))
        {
            Vector3 moveDirection = Player.Instance.transform.position - this.transform.position;

            rigidbodyReference.velocity = moveDirection.normalized * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            var reflectionDirection = Vector3.Reflect(lastFrameVelocity.normalized, collision.GetContact(0).normal);
            var speed = lastFrameVelocity.magnitude / 2;

            rigidbodyReference.velocity = reflectionDirection * speed;
        }
    }

    public void SetSpeed (float speed)
    {
        this.speed = speed;
    }

    public void SetMass (float mass)
    {
        rigidbodyReference.mass = mass;
    }

}
