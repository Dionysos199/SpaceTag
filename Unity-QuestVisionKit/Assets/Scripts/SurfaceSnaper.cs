using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SurfaceSnapper : MonoBehaviour
{
    [SerializeField] private float raycastHeightOffset = 0.2f;
    [SerializeField] private float raycastDistance = 1f;
    public MRUKAnchor.SceneLabels labelFilter;
    void Start()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeightOffset;
        Ray ray = new Ray (rayOrigin,Vector3.down);
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        if (room.Raycast(ray, raycastDistance, LabelFilter.FromEnum(labelFilter), out RaycastHit hit, out MRUKAnchor anchor))
        {
            // Snap to surface
            transform.position = hit.point;

            // Align to surface normal
            transform.up = hit.normal;
        }
        else
        {
            Debug.LogWarning($"[SurfaceSnapper] No mesh hit for {gameObject.name}");
        }
    }
}
