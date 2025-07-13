using UnityEngine;

public class LookAtFollow : MonoBehaviour
{
    public Transform target;         // Assign this in the Inspector
    public Vector3 upDirection = Vector3.up;  // Optional: customize 'up'

    void Update()
    {
        if (target == null) return;
        Vector3 directionToTarget = target.position - transform.position;

        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-directionToTarget, Vector3.up);
            transform.rotation = targetRotation;
        }

    }
}
