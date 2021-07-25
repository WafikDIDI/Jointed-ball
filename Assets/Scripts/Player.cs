using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Singleton
    public static Player Instance;

    [SerializeField] private float originalSpeed;
    private float currentSpeed;

    private Vector2 moveVector;
    public Vector2 MoveVector => moveVector;

    private Rigidbody rigidbodyReference;
    
    [Space]
    [SerializeField] private float dragDistance;

    [Space]
    [SerializeField] private float kickForce;
    [SerializeField] [Range(0f, 1f)] private float kickCooldown;
    [SerializeField] [Range(0f, 2f)] private float controlsCooldown;

    [Space]
    [SerializeField] private float originalBallMass;
    [SerializeField] private float kickingBallMass;

    private bool isAbleToKick;
    public bool IsKickingTheBall { get; private set; }

    [SerializeField] private Animator animator;

    private void Awake ()
    {
        // Singleton Setup
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
        // Initializations
        moveVector = new Vector2();
        currentSpeed = originalSpeed;
        IsKickingTheBall = false;
        isAbleToKick = true;

        rigidbodyReference = GetComponent<Rigidbody>();

        Ball.Instance.SetMass(originalBallMass);
    }

    private void Update ()
    {
        KickTheBall();
        MoveInputs();
        DragBall();
        LookAtTheBall();
    }

    private void KickTheBall ()
    {
        // Kicking the ball if the player is able to
        // Changing the ball's mass so the player can kicking it, yet couldn't push it around
        if (Input.GetMouseButtonDown(0) && isAbleToKick && GameManager.Instance.IsGameRunning)
        {
            isAbleToKick = false;

            IsKickingTheBall = true;
            Ball.Instance.SetMass(kickingBallMass);

            Vector3 kickingDirection = (Ball.Instance.transform.position - this.transform.position).normalized;
            rigidbodyReference.AddForce(kickingDirection * kickForce, ForceMode.Impulse);

            StartCoroutine(TimerRoutine(kickCooldown, () => isAbleToKick = true));

            StartCoroutine(TimerRoutine(controlsCooldown, () =>
            {
                IsKickingTheBall = false;
                Ball.Instance.SetMass(originalBallMass);
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
        // If the player is out of the drag distance he drags the ball, by given the ball speed the ball follows
        if (Vector3.Distance(this.transform.position, Ball.Instance.transform.position) > dragDistance)
        {
            if (currentSpeed == originalSpeed)
            {
                currentSpeed = originalSpeed / 2;
                Ball.Instance.SetSpeed(originalSpeed / 2);
            }
        }
        else
        {
            if (currentSpeed == originalSpeed / 2)
            {
                currentSpeed = originalSpeed;
                Ball.Instance.SetSpeed(0);
            }
        }
    }

    void LookAtTheBall ()
    {
        var positionToLookAt = new Vector3
        {
            x = Ball.Instance.transform.position.x,
            y = this.transform.position.y,
            z = Ball.Instance.transform.position.z,
        };

        this.transform.LookAt(positionToLookAt);
    }

    private void MoveInputs ()
    {
        // Collecting inputs
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate ()
    {
        /* If the player isn't kicking the ball, he moves with controls
           else if he did kick the ball, and it's getting out of his range he follows it (Ball dragging the player) */
        if (IsKickingTheBall == false)
        {
            Vector3 moveDirection = new Vector3(moveVector.x, 0, moveVector.y);
            rigidbodyReference.velocity = moveDirection * currentSpeed * Time.deltaTime;

            // Animate based on inputs
            animator.SetFloat("Vertical", Instance.MoveVector.x);
            animator.SetFloat("Horizontal", Instance.MoveVector.y);
        }
        else if (Vector3.Distance(this.transform.position, Ball.Instance.transform.position) > dragDistance)
        {
            Vector3 moveDirection = Ball.Instance.transform.position - this.transform.position;

            var speed = Ball.Instance.RigidbodyReference.velocity.magnitude;

            rigidbodyReference.velocity = moveDirection.normalized * speed;

            // Animate to move towards the ball
            animator.SetFloat("Vertical", 1);
            animator.SetFloat("Horizontal", 0);
        }
    }

    private void OnCollisionStay (Collision collision)
    {
        // To stop the player just after kicking the ball, looks better, feels better.
        if (collision.gameObject.CompareTag("Ball"))
        {
            rigidbodyReference.velocity = Vector3.zero;
        }
    }
}
