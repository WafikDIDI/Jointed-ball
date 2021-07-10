using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball Instance;

    private float speed;
    public float Speed => speed; 

    private Rigidbody rigidbodyReference;
    public Rigidbody RigidbodyReference => rigidbodyReference;

    private Vector3 lastFrameVelocity;

    [SerializeField] private float bounceForce;

    [Space]
    [SerializeField] private float noiseMinAngle;
    [SerializeField] private float noiseMaxAngle;

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
        rigidbodyReference = GetComponent<Rigidbody>();

        rigidbodyReference.AddForce(bounceForce * Vector3.up, ForceMode.Impulse);
    }

    private void Update ()
    {
        lastFrameVelocity = rigidbodyReference.velocity;
    }

    private void FixedUpdate ()
    {
        // Checks if it can be dragged by the player and if it has the speed to do so
        if ((speed > 0) && (Player.Instance.IsKickingTheBall == false))
        {
            Vector3 moveDirection = Player.Instance.transform.position - this.transform.position;

            rigidbodyReference.velocity += moveDirection.normalized * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        // Bouncing off walls
        if (collision.gameObject.tag != "Player")
        {
            var reflectionDirection = Vector3.Reflect(lastFrameVelocity.normalized, collision.GetContact(0).normal);
            reflectionDirection += AddNoiseOnAngle(noiseMinAngle, noiseMaxAngle);

            rigidbodyReference.AddForce(bounceForce * reflectionDirection.normalized, ForceMode.Impulse);
        }
    }

    // Speed Setter
    public void SetSpeed (float speed)
    {
        this.speed = speed;

        if (speed == 0)
        {
            rigidbodyReference.velocity = Vector2.zero;
        }
    }

    // Mass Setter
    public void SetMass (float mass)
    {
        rigidbodyReference.mass = mass;
    }

    // Generates a random vector that could be added to a direction to make Imperfect reflection
    Vector3 AddNoiseOnAngle (float min, float max)
    {
        // Find random angle between min & max inclusive
        float xNoise = UnityEngine.Random.Range(min, max);
        float yNoise = UnityEngine.Random.Range(min, max);
        float zNoise = UnityEngine.Random.Range(min, max);

        // Convert Angle to Vector3
        Vector3 noise = new Vector3(
          Mathf.Sin(2 * Mathf.PI * xNoise / 360),
          Mathf.Sin(2 * Mathf.PI * yNoise / 360),
          Mathf.Sin(2 * Mathf.PI * zNoise / 360)
        );

        return noise;
    }
}
