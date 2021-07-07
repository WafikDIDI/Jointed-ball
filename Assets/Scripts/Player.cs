using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float originalSpeed;
    private float currentSpeed;
    private Vector2 moveVector;

    private Rigidbody rigidbodyReference;

    [Space]
    [SerializeField] private Transform ballTransform;
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
    }

    private void KickTheBall ()
    {
        if (Input.GetMouseButtonDown(0) && isAbleToKick)
        {
            isAbleToKick = false;

            IsKickingTheBall = true;
            Ball.Instance.SetMass(kickingBallMass);

            Vector3 kickingDirection = (ballTransform.position - this.transform.position).normalized;
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
        if (Vector3.Distance(this.transform.position, ballTransform.position) > dragDistance)
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
            rigidbodyReference.velocity = Ball.Instance.RigidbodyReference.velocity;
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
