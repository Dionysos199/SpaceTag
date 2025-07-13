using UnityEngine;

public class MenuLookAtEye : MonoBehaviour
{
    [SerializeField] private Transform userCamera;  // Assign Main Camera or VR Head Camera

    private void LateUpdate()
    {
        if (userCamera == null) return;

        // FULLY rotate the menu to face the user's view — NO axis restrictions
        transform.LookAt(userCamera);
    }
}
