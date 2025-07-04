using System.Collections;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public int balloonCount = 20;

    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(5, 0, 5);
    public float floorOffsetY = 0.1f; // Lift balloons slightly off the floor

    void Start()
    {
        SpawnBalloons();
    }

    void SpawnBalloons()
    {
        for (int i = 0; i < balloonCount; i++)
        {
            Vector3 randomPos = GetRandomPointOnFloor();
            Instantiate(balloonPrefab, randomPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointOnFloor()
    {
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float z = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);
        Vector3 basePos = spawnAreaCenter + new Vector3(x, 0, z);

        // Use raycast to place balloon exactly on floor
        if (Physics.Raycast(basePos + Vector3.up * 5, Vector3.down, out RaycastHit hit, 10f))
        {
            return new Vector3(hit.point.x, hit.point.y + floorOffsetY, hit.point.z);
        }
        else
        {
            // If no hit, fallback to flat plane
            return basePos + new Vector3(0, floorOffsetY, 0);
        }
    }

    // Optional: visualize spawn area in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(spawnAreaCenter + Vector3.up * 0.1f, spawnAreaSize);
    }
}
