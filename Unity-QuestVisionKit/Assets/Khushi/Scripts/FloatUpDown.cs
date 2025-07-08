using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float amplitude = 0.5f;     // Up-down movement amount
    public float frequency = 1f;       // Up-down speed
    public float rotationSpeed = 50f;  // Degrees per second

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Float up and down
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // Rotate continuously around Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
