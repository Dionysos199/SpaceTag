using System.Collections;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class CeilingQuadSpawner : MonoBehaviour
{
   [Header("Ceiling Quad Settings")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float offsetBelowCeiling = 0.15f;
    [SerializeField] private bool overrideCeilingScale = true;

    [Header("Table Jar Settings")]
    [SerializeField] private GameObject jarPrefab;
    [SerializeField] private float offsetAboveTable = 0.1f;
    [SerializeField] private bool overrideJarScale = true;

    private void Start()
    {
        Debug.Log("🔄 Waiting for MRUK scene to load...");
        MRUK.Instance.RegisterSceneLoadedCallback(OnSceneLoaded);
    }

    private void OnSceneLoaded()
    {
        Debug.Log("✅ MRUK scene loaded.");
        Debug.Log($"🧠 Rooms found: {MRUK.Instance.Rooms.Count}");

        foreach (var room in MRUK.Instance.Rooms)
        {
            Debug.Log($"📂 Room: {room.name} | Anchors: {room.Anchors.Count}");

            foreach (var anchor in room.Anchors)
            {
                string labelList = anchor.Label.ToString();
                Debug.Log($"📦 Anchor '{anchor.name}' has labels: {labelList}");

                // Handle Ceiling
                if ((anchor.Label & MRUKAnchor.SceneLabels.CEILING) != 0)
                {
                    SpawnBelowCeiling(anchor);
                }

                // Handle Table
                if ((anchor.Label & MRUKAnchor.SceneLabels.TABLE) != 0)
                {
                    SpawnJarOnTable(anchor);
                }
            }
        }
    }

    private void SpawnBelowCeiling(MRUKAnchor anchor)
    {
        GameObject obj = Instantiate(ceilingPrefab);
        Vector3 spawnPos;

        if (anchor.VolumeBounds.HasValue)
        {
            Bounds bounds = anchor.VolumeBounds.Value;
            Vector3 localSpawnPos = new Vector3(0, -bounds.extents.y - offsetBelowCeiling, 0);
            obj.transform.parent = anchor.transform;
            obj.transform.localPosition = localSpawnPos;
            Debug.Log($"🟥 Ceiling: Spawned using VolumeBounds at local: {localSpawnPos}, world: {obj.transform.position}");
        }
        else
        {
            spawnPos = anchor.transform.position - anchor.transform.up * offsetBelowCeiling;
            obj.transform.position = spawnPos;
            obj.transform.rotation = Quaternion.LookRotation(Vector3.up);
            Debug.Log($"🟥 Ceiling: Fallback spawn below anchor '{anchor.name}' at world position: {spawnPos}");
        }

        if (overrideCeilingScale)
        {
            obj.transform.localScale = new Vector3(20f, 20f, 20f); // Visible size
        }

        var rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.magenta;
        }

        obj.name = "CeilingQuad_" + anchor.name;
    }

    private void SpawnJarOnTable(MRUKAnchor anchor) 
    {
        if (!anchor.VolumeBounds.HasValue)
        {
            Debug.LogWarning($"❌ Anchor '{anchor.name}' has no VolumeBounds.");
            return;
        }

        GameObject jar = Instantiate(jarPrefab);
        jar.name = $"Jar_{anchor.name}";

        Bounds bounds = anchor.VolumeBounds.Value;

        // Center of the anchor in world space
        Vector3 worldCenter = anchor.transform.TransformPoint(bounds.center);

        // Move up slightly from the anchor surface
        float liftAbove = bounds.extents.y + offsetAboveTable;

        // Push slightly in front
        float pushForward = 0.1f;
        Vector3 forward = anchor.transform.forward;

        Vector3 spawnPos = worldCenter + Vector3.up * liftAbove + forward * pushForward;
        jar.transform.position = spawnPos;

        // Rotation — upright, facing same direction as table front
        Vector3 up = Vector3.up;

        // If forward and up too similar, use right instead
        if (Vector3.Dot(forward, up) > 0.9f)
        {
            forward = anchor.transform.right;
        }

        jar.transform.rotation = Quaternion.LookRotation(forward, up);

        // Scale override
        if (overrideJarScale)
        {
            jar.transform.localScale = Vector3.one * 1.5f;
        }

        Debug.Log($"🟢 Jar spawned at {spawnPos} on anchor '{anchor.name}'");
    }
}
