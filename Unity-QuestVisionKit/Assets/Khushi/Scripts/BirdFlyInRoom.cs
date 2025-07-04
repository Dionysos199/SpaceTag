using System.Collections;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class BirdFlyInRoom : MonoBehaviour
{
    
   public float speed = 1.5f;
    public float changeDirectionTime = 3f;
    private float timer;
    [SerializeField] Transform leftPalm;
    [SerializeField] Transform rightPalm;

    public float handTogetherThreshold = 0.1f;
    public float landingSmoothTime = 1f;
    private bool isTakingOff = false;
    private bool birdIsLanding = false;
    private Vector3 landingVelocity;
    private Vector3 targetPosition;
    private Bounds roomBounds;

    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(OnMRUKReady);
    }

    void OnMRUKReady()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        if (room == null)
        {
            Debug.LogWarning("❌ No MRUK room found.");
            return;
        }

        // Add colliders to MRUK anchors if missing
        foreach (var anchor in room.Anchors)
        {
            if (anchor.GetComponent<Collider>() == null)
            {
                var meshFilter = anchor.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    anchor.gameObject.AddComponent<MeshCollider>();
                }
                else
                {
                    anchor.gameObject.AddComponent<BoxCollider>();
                }
            }
        }

        roomBounds = CalculateRoomBounds(room);
        Debug.Log($"📦 Room bounds calculated: center={roomBounds.center}, size={roomBounds.size}");
        // Move bird to a better-looking initial position (e.g., center of the room, a bit above ground)
        Vector3 startPos = roomBounds.center;
        startPos.y = Mathf.Clamp(startPos.y + 1.5f, roomBounds.min.y + 1.5f, roomBounds.max.y - 0.5f);
        transform.position = startPos;

// Look in a neutral direction (e.g., forward or random flat direction)
        Vector3 lookDirection = Vector3.forward;
        lookDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * lookDirection; // random flat look
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        
        PickNewTarget();
    }

    void Update()
    {
        if (roomBounds.size == Vector3.zero) return;
        if (leftPalm != null && rightPalm != null)
        {
            float handsDistance = Vector3.Distance(leftPalm.position, rightPalm.position);

            if (handsDistance < handTogetherThreshold)
            {
                Vector3 midPoint = (leftPalm.position + rightPalm.position) / 2f;
                LandOnHands(midPoint);
                return; // ⛔ Skip flying logic when bird is landing
            }
           /* else if (birdIsLanding)
            {
                birdIsLanding = false; // Resume flying next frame
                PickNewTarget();       // Pick new flight target
            }*/
           else if (birdIsLanding)
           {
               birdIsLanding = false;
               StartCoroutine(SmoothTakeoff());
           }
        } 
        void LandOnHands(Vector3 targetPosition)
{
    if (!birdIsLanding)
    {
        birdIsLanding = true;
        // Optional: play chirp, animation etc.
    }

    transform.position = Vector3.SmoothDamp(
        transform.position,
        targetPosition,
        ref landingVelocity,
        landingSmoothTime
    );

   /* // Look in flat forward direction between hands
    Vector3 flatForward = (rightPalm.position - leftPalm.position).normalized;
    flatForward.y = 0;

    if (flatForward.sqrMagnitude > 0.001f)
    {
        Quaternion lookRot = Quaternion.LookRotation(flatForward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
    }
    */
   
   // New logic — bird faces the user (camera), but stays level
   Vector3 toCamera = Camera.main.transform.position - transform.position;
   toCamera.y = 0; // flatten to horizontal

   if (toCamera.sqrMagnitude > 0.001f)
   {
       Quaternion lookRot = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
       transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
   }
    
}
        if (!isTakingOff)
        {
            timer += Time.deltaTime;
            if (timer > changeDirectionTime)
            {
                PickNewTarget();
                timer = 0;
            }

            Vector3 move = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = ClampToBounds(move);

            Quaternion lookRot = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
        }
    }
    
    IEnumerator SmoothTakeoff()
    {
        isTakingOff = true;

        Vector3 takeoffTarget = transform.position + new Vector3(0, 0.5f, 0);
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(transform.position, takeoffTarget, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        PickNewTarget(); // ✅ Get next target *before* rotating
        yield return RotateSmoothlyToTarget(); // ✅ Smoothly rotate to that target

        isTakingOff = false;
    }
    
    IEnumerator RotateSmoothlyToTarget()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

        float rotateDuration = 0.4f;
        float rotateElapsed = 0f;

        while (rotateElapsed < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, rotateElapsed / rotateDuration);
            rotateElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
    
    void LandOnHands(Vector3 targetPosition)
    {
        if (!birdIsLanding)
        {
            birdIsLanding = true;
            // Optional: play chirp, animation etc.
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref landingVelocity,
            landingSmoothTime
        );

        // Look in flat forward direction between hands
        Vector3 flatForward = (rightPalm.position - leftPalm.position).normalized;
        flatForward.y = 0;

        if (flatForward.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(flatForward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
        }
    }
    void PickNewTarget()
    {
        float padding = 0.3f;
        float minDistance = 1.0f; // ✅ Adjust this as needed (e.g. 1 meter minimum flight)

        Vector3 min = roomBounds.min + Vector3.one * padding;
        Vector3 max = roomBounds.max - Vector3.one * padding;

        float yMin = Mathf.Clamp(min.y + 1.5f, min.y, max.y - padding);
        float yMax = Mathf.Clamp(max.y - 0.5f, yMin, max.y);

        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(yMin, yMax),
                Random.Range(min.z, max.z)
            );

            Vector3 direction = candidate - transform.position;
            float distance = direction.magnitude;

            if (distance < minDistance)
                continue; // 🔁 Skip if too close

            // Use SphereCast for obstacle detection
            if (!Physics.SphereCast(transform.position, 0.2f, direction.normalized, out RaycastHit hit, distance - 0.2f))
            {
                targetPosition = candidate;
                return;
            }
        }

        // If all attempts fail, stay at current position
        targetPosition = transform.position;
    }

    Bounds CalculateRoomBounds(MRUKRoom room)
    {
        var allAnchors = room.Anchors;
        if (allAnchors.Count == 0) return new Bounds(Vector3.zero, Vector3.zero);

        Bounds bounds = new Bounds(allAnchors[0].transform.position, Vector3.zero);
        foreach (var anchor in allAnchors)
        {
            bounds.Encapsulate(anchor.transform.position);
        }
        return bounds;
    }

    Vector3 ClampToBounds(Vector3 position)
    {
        float padding = 0.3f;
        Vector3 min = roomBounds.min + Vector3.one * padding;
        Vector3 max = roomBounds.max - Vector3.one * padding;

        return new Vector3(
            Mathf.Clamp(position.x, min.x, max.x),
            Mathf.Clamp(position.y, min.y + 1.5f, max.y - 0.5f),
            Mathf.Clamp(position.z, min.z, max.z)
        );
    }
}
