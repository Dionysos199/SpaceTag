using UnityEngine;

public class CanvasFollow : MonoBehaviour
{
    public Transform leader;
    public Transform follower;
    public Vector3 localOffset = new Vector3(1f, 0, 0);
    public Vector3 followerRotation = Vector3.zero;
   
    void Update()
    {
        if (leader != null && follower != null)
        {
            Vector3 worldOffset = leader.TransformDirection(localOffset);
            follower.position = leader.position + worldOffset;
            follower.rotation = leader.rotation * Quaternion.Euler(followerRotation);
        }
    }
}
