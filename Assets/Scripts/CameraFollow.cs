using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;

    [Space]
    [SerializeField] private Vector3 offset;

    void LateUpdate ()
    {
        // Mixing the target position on X and Z, with the camera's Y position to keep the same height
        Vector3 newPosition = new Vector3
        {
            x = targetToFollow.transform.position.x,
            y = this.transform.position.y,
            z = targetToFollow.transform.position.z
        };

        // Assigning the new position with an offset vector to the camera
        this.transform.position = newPosition + offset;
    }
}
