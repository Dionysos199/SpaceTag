using LearnXR.Core.Utilities;
using Meta.XR.MRUtilityKit;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ControllerRaycast : MonoBehaviour
{
    public Transform rayStartPoint;
    public float rayLength = 5;
    public MRUKAnchor.SceneLabels labelFilter;

    private SpatialLogger logger;
    private LineRenderer lineRenderer;

    void Start()
    {
        logger = FindFirstObjectByType<SpatialLogger>();
        lineRenderer = GetComponent<LineRenderer>();

        // Setup LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);

        // Set line positions so it shows in Game view and builds
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * rayLength);

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        bool hasHit = room.Raycast(ray, rayLength, LabelFilter.FromEnum(labelFilter), out RaycastHit hit, out MRUKAnchor anchor);
        
        if (hasHit)
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            string label = anchor.AnchorLabels[0];

            logger.LogInfo("hit position " + hit.transform.position);
            logger.LogInfo("hit normal " + hit.normal);
            logger.LogInfo($"{label}");

            // Optional: draw the normal as a small line
            Debug.DrawRay(hitPoint, hitNormal * 0.2f, Color.red); // only visible in editor
        }
        else
        {
            //logger.LogInfo("nothing was hit");
        }
    }
}
