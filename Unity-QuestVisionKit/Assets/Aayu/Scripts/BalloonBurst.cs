using UnityEngine;

public class BalloonBurst : MonoBehaviour
{
    public GameObject confettiEffectPrefab;
    public GameObject chocolateRewardPrefab;
    public AudioClip popSound;
    public float chocolateSpawnHeight = 0.1f;

    private bool isPopped = false;

    void OnTriggerEnter(Collider other)
    {
        if (isPopped) return;

        if (other.CompareTag("Pin"))
        {
            isPopped = true;

            // Spawn confetti
            if (confettiEffectPrefab != null)
            {
                Instantiate(confettiEffectPrefab, transform.position, Quaternion.identity);
            }

            // Spawn chocolate reward
            if (chocolateRewardPrefab != null)
            {
                Vector3 spawnPos = transform.position;
                spawnPos.y += chocolateSpawnHeight;
                Instantiate(chocolateRewardPrefab, spawnPos, Quaternion.identity);
            }

            // Play pop sound
            if (popSound != null)
            {
                AudioSource.PlayClipAtPoint(popSound, transform.position);
            }

            // Destroy balloon
            Destroy(gameObject);
        }
    }
}
