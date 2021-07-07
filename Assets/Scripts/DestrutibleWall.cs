using System.Collections.Generic;
using UnityEngine;

public class DestrutibleWallPart : MonoBehaviour
{
    private List<Rigidbody> rigidbodies;
    private Ball ballReference;

    [SerializeField] private Transform vfxTransform;

    private void Start ()
    {
        rigidbodies = new List<Rigidbody>();
        foreach (Transform transform in this.transform)
        {
            rigidbodies.Add(transform.GetComponent<Rigidbody>());
        }

        ballReference = FindObjectOfType<Ball>();
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            vfxTransform.gameObject.SetActive(true);
            vfxTransform.parent = null;

            Destroy(this.gameObject);
        }   
    }
}
