using System.Collections;
using TMPro;
using UnityEngine;

public class HandStarDetector : MonoBehaviour
{
    [SerializeField] private GameObject starCanvasPrefab;
    [SerializeField] private Transform spawnPoint; // Where to instantiate it
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float popDuration = 0.3f;
    [SerializeField] private float delayBeforePop = 5f;
    [SerializeField] private AudioSource StarAudioSource;
    

    private GameObject spawnedPrefab;
    private GameObject starObject;
    private GameObject starCanvas;
    private TextMeshProUGUI starText;
    private int starTouchCount = 0;
    private float typingSpeed = 0.05f; // seconds between each character
    void Start()
    {
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(delayBeforePop);

        // Instantiate prefab
        spawnedPrefab = Instantiate(starCanvasPrefab, spawnPoint.position, Quaternion.identity);

        //play sound
        if (StarAudioSource != null)
        {
            StarAudioSource.Play();
        }
        
        // Get references to child objects
        starObject = spawnedPrefab.transform.Find("StarObject").gameObject;
        starCanvas = spawnedPrefab.transform.Find("StarCanvas").gameObject;
        starText = starCanvas.GetComponentInChildren<TextMeshProUGUI>();

        // Set up
        starCanvas.SetActive(false);
        starObject.SetActive(true);
        StartCoroutine(PopEffect(starObject.transform));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            starTouchCount++;

            if (starTouchCount == 1)
            {
                starCanvas.SetActive(true);
                if (starText != null)
                {
                    StartCoroutine(TypeTextEffect("One day at a time. I’m proud of you. Rest now ...Good Night!"));
                }
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

    private IEnumerator TypeTextEffect(string fullText)
    {
        starText.text = "";
        foreach (char c in fullText)
        {
            starText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
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
