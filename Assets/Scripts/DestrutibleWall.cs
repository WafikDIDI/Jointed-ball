using System.Collections.Generic;
using UnityEngine;

public class DestrutibleWall : MonoBehaviour
{
    [SerializeField] private Transform vfxTransform;

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
