using UnityEngine;
using System.Collections;

public class BalloonFloat : MonoBehaviour
{
    public float buoyancyForce = .1f;
    public float bounceDamping = 0.4f;
    public float ceilingCheckDistance = 5f;

    private Rigidbody rb;
    private float ceilingY;
    private bool ceilingFound = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        StartCoroutine(FindCeiling());
    }

    void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }

        if (ceilingFound && transform.position.y >= ceilingY)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y *= -bounceDamping;
            rb.linearVelocity = velocity;

            Vector3 pos = transform.position;
            pos.y = ceilingY;
            transform.position = pos;

            if (Mathf.Abs(velocity.y) < 0.05f)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = false;
            }
        }
    }

    IEnumerator FindCeiling()
    {
        // Wait a bit for MRUK to finish loading room mesh
        yield return new WaitForSeconds(1.5f);

        Ray ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit, ceilingCheckDistance))
        {
            // If it hits something above, assume it's the ceiling
            ceilingY = hit.point.y;
            ceilingFound = true;
        }
        else
        {
            // If nothing hit, use a fallback ceiling height
            ceilingY = transform.position.y + 3.0f;
            ceilingFound = true;
        }
    }
}

/*
using UnityEngine;
using System.Collections;

public class BalloonFloat : MonoBehaviour
{
    public float buoyancyForce = 1.5f;
    public float bounceDamping = 0.4f;
    public float ceilingCheckDistance = 5f;

    private Rigidbody rb;
    private float ceilingY;
    private bool ceilingFound = false;
    private bool canFloat = false; // New flag

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        StartCoroutine(FindCeiling());
        StartCoroutine(EnableFloatAfterDelay());
    }

    void FixedUpdate()
    {
        if (canFloat && !rb.isKinematic)
        {
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }

        if (ceilingFound && transform.position.y >= ceilingY)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y *= -bounceDamping;
            rb.linearVelocity = velocity;

            Vector3 pos = transform.position;
            pos.y = ceilingY;
            transform.position = pos;

            if (Mathf.Abs(velocity.y) < 0.05f)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }

    IEnumerator EnableFloatAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Delay before balloon floats
        canFloat = true;
    }

    IEnumerator FindCeiling()
    {
        yield return new WaitForSeconds(1.5f);

        Ray ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out RaycastHit hit, ceilingCheckDistance))
        {
            ceilingY = hit.point.y;
        }
        else
        {
            ceilingY = transform.position.y + 3.0f;
        }

        ceilingFound = true;
    }
}
*/