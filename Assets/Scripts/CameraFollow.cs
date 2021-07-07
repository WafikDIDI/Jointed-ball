using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform firstTarget;
    [SerializeField] private Transform secondTarget;

    [Space]
    [SerializeField] private Vector3 offset;

    void LateUpdate()
    {
        Vector3 newPosition = Vector3.zero;
        newPosition.x = (firstTarget.transform.position.x + secondTarget.transform.position.x) / 2;
        newPosition.y = this.transform.position.y;
        newPosition.z = (firstTarget.transform.position.z + secondTarget.transform.position.z) / 2;

        this.transform.position = newPosition + offset;
    }
}
