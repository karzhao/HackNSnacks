using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Assign the player in the Inspector
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float fixedYPosition = 0f; // Set this in the Inspector or in Start()

    void Start()
    {
        // Set the camera's initial Y position based on its current position
        fixedYPosition = transform.position.y;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow the target's X position, keep Y fixed
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, fixedYPosition, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            transform.position = new Vector3(smoothedPosition.x, fixedYPosition, transform.position.z);
        }
    }
}
