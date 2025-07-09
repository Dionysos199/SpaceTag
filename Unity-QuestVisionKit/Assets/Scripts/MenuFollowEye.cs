using UnityEngine;

public class MenuFollowEye : MonoBehaviour
{
    [SerializeField] private Transform userCamera;  // Usually your Main Camera (or VR Head)
    [SerializeField] private float followDistance = 2f;  // Distance from camera
    [SerializeField] private float smoothSpeed = 5f;     // Smoothness of following

    private void LateUpdate()
    {
        if (userCamera == null) return;

        // Target position in front of the user
        Vector3 targetPosition = userCamera.position + userCamera.forward * followDistance;

        // Smoothly move menu toward target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Make menu face the camera (optional)
        transform.LookAt(userCamera);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);  // Y-axis only (optional, for horizontal facing)
    }
}
