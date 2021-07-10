using UnityEngine;

public class DestrutibleWall : MonoBehaviour
{
    [SerializeField] private Transform vfxTransform;

    private void OnCollisionEnter (Collision collision)
    {
        // Checks if the ball hit the wall, and if it did plays a VFX and destorys itself
        if (collision.gameObject.CompareTag("Ball"))
        {
            vfxTransform.gameObject.SetActive(true);
            vfxTransform.parent = null;

            Destroy(this.gameObject);
        }
    }
}
