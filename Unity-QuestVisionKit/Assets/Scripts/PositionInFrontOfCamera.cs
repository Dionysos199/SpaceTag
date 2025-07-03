using UnityEngine;

public class PositionInFrontOfCamera : MonoBehaviour
{
    [SerializeField] float distance = 3f;
    
    public void PositionDialogueInFrontOfCamera()
    {
        // Get the main camera
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("No main camera found in the scene.");
            return;
        }

        // Calculate target position in front of the camera
        Vector3 targetPosition = cam.transform.position + cam.transform.forward * distance;
        transform.position = targetPosition;

        // Make the dialogue face the camera
        transform.LookAt(cam.transform.position);

        //// Optional: Flip if the object faces backward
        //transform.Rotate(0, 180f, 0);
    }

}
