using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private Player playerReference;
    
    private float speed;

    private Rigidbody rigidbodyReference;
    public Rigidbody RigidbodyReference => rigidbodyReference;

    [Space]
    [SerializeField] private float destructionForce;
    public float DestructionForce => destructionForce;

    private void Start ()
    {
        rigidbodyReference = GetComponent<Rigidbody>();
        playerReference = playerTransform.GetComponent<Player>();
    }

    private void FixedUpdate ()
    {
        if ((speed > 0) && (playerReference.IsKickingTheBall == false))
        {
            Vector3 moveDirection = playerTransform.position - this.transform.position;

            rigidbodyReference.velocity = moveDirection.normalized * speed * Time.deltaTime;
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
