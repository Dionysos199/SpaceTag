using System.Collections;
using UnityEngine;

public class HandStarDetector : MonoBehaviour
{
    [SerializeField] private GameObject starCanvas;    // UI Canvas to activate
    [SerializeField] private GameObject starObject;    // Star GameObject to enable and pop
    [SerializeField] private float popScale = 1.2f;     // How big it scales
    [SerializeField] private float popDuration = 0.3f;  // Total pop time
    [SerializeField] private float delayBeforePop = 5f; // Delay in seconds before star appears
    private int starTouchCount = 0;
    void Start()
    {
        // Add Box Collider and set it as trigger
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }
        boxCollider.isTrigger = true;

        // Hide star at start
        if (starObject != null)
        {
            starObject.SetActive(false);
            StartCoroutine(DelayedPop());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            starTouchCount++;

            if (starTouchCount == 1)
            {
                starCanvas.SetActive(true);
                Debug.Log("⭐ First touch – canvas shown");
            }
            else if (starTouchCount == 2)
            {
                starCanvas.SetActive(false);
                Debug.Log("🛑 Second touch – canvas hidden");
            }
            else
            {
                Debug.Log("🚫 Star touched again – no action");
            }
        }
    }

    private IEnumerator DelayedPop()
    {
        yield return new WaitForSeconds(delayBeforePop);

        // Show and animate the star
        starObject.SetActive(true);
        yield return StartCoroutine(PopEffect(starObject.transform));
    }

    private IEnumerator PopEffect(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 bigScale = originalScale * popScale;

        float halfDuration = popDuration / 2f;
        float t = 0f;

        // Scale up
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale, bigScale, t / halfDuration);
            yield return null;
        }

        // Scale down
        t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(bigScale, originalScale, t / halfDuration);
            yield return null;
        }

        target.localScale = originalScale;
    }
}
