using Dreamteck.Splines;
using LearnXR.Core.Utilities;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction.Input;
using System.Collections;
using UnityEngine;

public class AlignToSurface : MonoBehaviour
{
    public float maxRayDistance = 2f;
    public MRUKAnchor. SceneLabels labelFilter; // Default: all layers
    private SpatialLogger logger;
    [SerializeField] GameObject cat;
    void Start()
    {
        logger = FindAnyObjectByType<SpatialLogger>();

        if (MRUK.Instance != null)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(AlignToSurfaceOnRoomReady);
        }
        else
        {
            logger.LogInfo("MRUK instance is null!");
        }
    }
    IEnumerator  waitForEndOfFrame(Vector3 pos, Quaternion rotation)
    {

        yield return new WaitForEndOfFrame();
        transform.position = pos;
        transform.rotation = rotation;
    }
    public void AlignToSurfaceOnRoomReady()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        if (Physics.Raycast(ray,  out RaycastHit hit, maxRayDistance))
        {

            logger.LogInfo("Hit point! Distance: " + hit.distance + ", Position: " + hit.point);

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            StartCoroutine(waitForEndOfFrame(hit.point, rotation));
        }
        else
        {
            logger.LogInfo("AlignToSurface: No surface found below prefab.");
        }
    }

    void Update()
    {
        float rayLength = 5;
        Transform rayStartPoint = transform;
        Ray ray = new Ray(rayStartPoint.position, -transform.up);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        // Set line positions so it shows in Game view and builds
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * rayLength);

        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
    
        if (room.Raycast(ray, maxRayDistance, LabelFilter.FromEnum(labelFilter), out RaycastHit hit, out MRUKAnchor anchor))
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

