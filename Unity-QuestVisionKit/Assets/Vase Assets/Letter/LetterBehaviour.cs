using System.Collections;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour
{
 public GameObject letterPrefab;
    public OVRHand leftHand;
    public OVRHand rightHand;
    public Transform spawnPoint;
    public Animator envelopeAnimation;
    
    private bool letterSpawned = false;
    private bool wasLeftPinching = false;
    private bool wasRightPinching = false;
    private bool animationComplete = false;
    
    void Update()
    {
        // Check if animation is complete
        if (!animationComplete && envelopeAnimation != null)
        {
            animationComplete = envelopeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
            if (animationComplete)
            {
                Debug.Log("Animation complete - pinch now available!");
            }
        }
        
        // Only check for pinching after animation is done
        if (animationComplete && !letterSpawned)
        {
            CheckPinchGesture();
        }
    }
    
    void CheckPinchGesture()
    {
        // Check left hand pinching
        if (leftHand.IsTracked)
        {
            bool isLeftPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            
            if (!wasLeftPinching && isLeftPinching)
            {
                Debug.Log("Left hand pinch detected!");
                SpawnLetter(leftHand.transform.position);
            }
            wasLeftPinching = isLeftPinching;
        }
        
        // Check right hand pinching
        if (rightHand.IsTracked)
        {
            bool isRightPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            
            if (!wasRightPinching && isRightPinching)
            {
                Debug.Log("Right hand pinch detected!");
                SpawnLetter(rightHand.transform.position);
            }
            wasRightPinching = isRightPinching;
        }
    }
    
    void SpawnLetter(Vector3 handPosition)
    {
        Debug.Log("Spawning letter");
        Vector3 spawnPos = spawnPoint ? spawnPoint.position : handPosition;
        GameObject letter = Instantiate(letterPrefab, spawnPos, Random.rotation);
        letterSpawned = true;
    }
}
