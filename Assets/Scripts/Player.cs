using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float originalSpeed;
    private float currentSpeed;
    private Vector2 moveVector;

    private Rigidbody rigidbodyReference;
    
    private Ball ballReference;

    [Space]
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float dragDistance;

    [Space]
    [SerializeField] private float kickForce;
    [SerializeField][Range(0f, 2f)] private float controlsCooldown;

    [Space]
    [SerializeField] private float originalBallMass;
    [SerializeField] private float kickingBallMass;

    public bool IsKickingTheBall { get; private set; }

    private void Start ()
    {
        moveVector = new Vector2();
        currentSpeed = originalSpeed;
        IsKickingTheBall = false;

        rigidbodyReference = GetComponent<Rigidbody>();
        ballReference = ballTransform.GetComponent<Ball>();
        ballReference.SetMass(originalBallMass);
    }

    private void Update ()
    {
        KickTheBall();
        MoveInputs();
        DragBall();
    }

    private void KickTheBall ()
    {
        if (Input.GetMouseButton(0))
        {
            IsKickingTheBall = true;
            ballReference.SetMass(kickingBallMass);

            Vector3 kickingDirection = (ballTransform.position - this.transform.position).normalized;
            rigidbodyReference.AddForce(kickingDirection * kickForce, ForceMode.Impulse);

            StartCoroutine(TimerRoutine(controlsCooldown, () =>
            {
                IsKickingTheBall = false;
                ballReference.SetMass(originalBallMass);
            }));
        }
    }

    private IEnumerator TimerRoutine (float timeToWait, Action actionToDo)
    {
        yield return new WaitForSeconds(timeToWait);

        if (actionToDo != null)
        {
            actionToDo();
        }
    }

    private void DragBall ()
    {
        if (Vector3.Distance(this.transform.position, ballTransform.position) > dragDistance)
        {
            if (currentSpeed == originalSpeed)
            {
                currentSpeed = originalSpeed / 2;
                ballReference.SetSpeed(originalSpeed / 2);
            }
        }
        else
        {
            if (currentSpeed == originalSpeed / 2)
            {
                currentSpeed = originalSpeed;
                ballReference.SetSpeed(0);
            }
        }
    }

    private void MoveInputs ()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate ()
    {
        if (IsKickingTheBall == false)
        {
            Vector3 moveDirection = new Vector3(moveVector.x, 0, moveVector.y);
            rigidbodyReference.velocity = moveDirection * currentSpeed * Time.deltaTime;
        }
        else if (Vector3.Distance(this.transform.position, ballTransform.position) > dragDistance)
        {
            rigidbodyReference.velocity = ballReference.RigidbodyReference.velocity;
        }
    }

    private void OnCollisionStay (Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            rigidbodyReference.velocity = Vector3.zero;
        }
    }
}
