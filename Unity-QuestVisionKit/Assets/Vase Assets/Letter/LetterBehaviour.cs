using System.Collections;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour
{
    public GameObject letterPrefab;
    public float animationDuration = 1f;
    
    private bool animationDone = false;
    private bool letterSpawned = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OVRHand>() && !animationDone && !letterSpawned)
        {
            StartCoroutine(PlayAnimation());
        }
        else if (other.GetComponent<OVRHand>() && animationDone && !letterSpawned)
        {
            SpawnLetter(other.transform.position);
        }
    }
    
    IEnumerator PlayAnimation()
    {
        // Simple shake animation
        Vector3 originalPos = transform.position;
        float timer = 0;
        
        while (timer < animationDuration)
        {
            transform.position = originalPos + Random.insideUnitSphere * 0.1f;
            timer += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPos;
        animationDone = true;
    }
    
    void SpawnLetter(Vector3 spawnPos)
    {
        GameObject letter = Instantiate(letterPrefab, spawnPos, Random.rotation);
        letter.AddComponent<OVRGrabbable>();
        letterSpawned = true;
        gameObject.SetActive(false);
    }
}
