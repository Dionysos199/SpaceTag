using System.Collections;
using Meta.XR.MRUtilityKit;
using TMPro;
using UnityEngine;

public class BirdFlyInRoom : MonoBehaviour
{
    
    public float speed = 1.5f;
    public float changeDirectionTime = 3f;
    private float timer;

    [SerializeField] Transform leftPalm;
    [SerializeField] Transform rightPalm;

    public float handTogetherThreshold = 1f;
    public float landingSmoothTime = 1f;
    
    private float startupGracePeriod = 2f;
    private float elapsedSinceStart = 0f;
    
    private bool birdIsLanding = false;
    private bool isTakingOff = false;

    private Vector3 landingVelocity;
    private Vector3 targetPosition;
    private Bounds roomBounds;

    private Animator animator;
    private Canvas birdCanvas;
    private TextMeshProUGUI messageText;
    private AudioSource typingAudio;
    void Start()
    {
        animator = GetComponent<Animator>();
        typingAudio = GetComponent<AudioSource>();
        birdCanvas = GetComponentInChildren<Canvas>(true); // true allows finding disabled canvas
       /* if (birdCanvas != null)
            birdCanvas.enabled = false; // make sure it starts hidden */
        if (birdCanvas != null)
        {
            birdCanvas.gameObject.SetActive(false); // ✅ This enables the object
            messageText = birdCanvas.GetComponentInChildren<TextMeshProUGUI>(true);
           // birdCanvas.enabled = true; // no delay
            Debug.Log("🎯 Canvas enabled immediately");
        } 
        
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

        foreach (var anchor in room.Anchors)
        {
            if (anchor.GetComponent<Collider>() == null)
            {
                var meshFilter = anchor.GetComponent<MeshFilter>();
                if (meshFilter != null)
                    anchor.gameObject.AddComponent<MeshCollider>();
                else
                    anchor.gameObject.AddComponent<BoxCollider>();
            }
        }

        roomBounds = CalculateRoomBounds(room);
        Vector3 startPos = roomBounds.center;
        startPos.y = Mathf.Clamp(startPos.y + 1.5f, roomBounds.min.y + 1.5f, roomBounds.max.y - 0.5f);
        transform.position = startPos;

        Vector3 lookDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        PickNewTarget();
    }

    void Update()
    {
        // ⏳ Wait for grace period before doing anything
        elapsedSinceStart += Time.deltaTime;
        if (elapsedSinceStart < startupGracePeriod) return;

        if (roomBounds.size == Vector3.zero) return;

        bool handsClose = false;

        if (leftPalm != null && rightPalm != null)
        {
            float handsDistance = Vector3.Distance(leftPalm.position, rightPalm.position);
            handsClose = handsDistance < handTogetherThreshold;

            if (handsClose)
            {
                Vector3 midPoint = (leftPalm.position + rightPalm.position) / 2f;
                LandOnHands(midPoint);
                return; // skip flying logic
            }

            if (birdIsLanding)
            {
                birdIsLanding = false;

                if (birdCanvas != null)
                {
                    birdCanvas.gameObject.SetActive(false);
                    Debug.Log("🔴 Canvas hidden on takeoff");
                }

                StartCoroutine(SmoothTakeoff());
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

            if (animator != null)
            {
                animator.SetBool("IsFlying", true);
                animator.SetBool("IsLanding", false);
            }
        }
    }

    void LandOnHands(Vector3 target)
    {
        float distanceToPalm = Vector3.Distance(transform.position, target);

        // Move bird smoothly toward the hand midpoint
        transform.position = Vector3.SmoothDamp(transform.position, target, ref landingVelocity, landingSmoothTime);

        // Rotate to face user
        Vector3 toCamera = Camera.main.transform.position - transform.position;
        toCamera.y = 0;
        if (toCamera.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 2f);
        }

        // ✅ Only when bird is VERY close to hand, play landing animation
        if (!birdIsLanding && distanceToPalm < 0.05f)
        {
            birdIsLanding = true;

            if (animator != null)
            {
                animator.SetBool("IsFlying", false);
                animator.SetBool("IsLanding", true); // ✅ TRIGGER LANDING ANIMATION
            }

            Debug.Log("✅ Bird has landed!");
            
            // ⏱️ Start delayed canvas enable
            if (birdCanvas != null)
            {
                StartCoroutine(EnableCanvasAfterDelay(1f));
            }
        }
    }

    IEnumerator EnableCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (birdCanvas != null && messageText != null)
        {
            birdCanvas.gameObject.SetActive(true);
            StartCoroutine(TypeMessage("You annoy me less than everyone else. That’s love."));
        }
    } 
    
    IEnumerator SmoothTakeoff()
    {
        isTakingOff = true;

        // ✅ Trigger flying animation FIRST
        if (animator != null)
        {
            animator.SetBool("IsLanding", false);
            animator.SetBool("IsFlying", true);
        }

        // Smooth vertical takeoff
        Vector3 takeoffTarget = transform.position + Vector3.up * 0.5f;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(transform.position, takeoffTarget, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isTakingOff = false;
        PickNewTarget(); // Continue flying
    }

    void PickNewTarget()
    {
        float padding = 0.3f;
        float minDistance = 1.0f;

        Vector3 min = roomBounds.min + Vector3.one * padding;
        Vector3 max = roomBounds.max - Vector3.one * padding;

        float yMin = Mathf.Clamp(min.y + 1.5f, min.y, max.y - padding);
        float yMax = Mathf.Clamp(max.y - 0.5f, yMin, max.y);

        for (int i = 0; i < 10; i++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(yMin, yMax),
                Random.Range(min.z, max.z)
            );

            Vector3 direction = candidate - transform.position;
            float distance = direction.magnitude;

            if (distance < minDistance) continue;

            if (!Physics.SphereCast(transform.position, 0.2f, direction.normalized, out RaycastHit hit, distance - 0.2f))
            {
                targetPosition = candidate;
                return;
            }
        }

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
    
    IEnumerator TypeMessage(string fullMessage, float delay = 0.05f)
    {
        messageText.text = "";

        if (typingAudio != null)
            typingAudio.Play();

        foreach (char c in fullMessage)
        {
            messageText.text += c;
            yield return new WaitForSeconds(delay);
        }

        if (typingAudio != null)
            typingAudio.Stop();
    }
}
