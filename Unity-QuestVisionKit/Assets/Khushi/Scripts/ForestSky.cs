using Meta.XR.MRUtilityKit;
using UnityEngine;

public class ForestSky : MonoBehaviour
{
    
    [Header("Ceiling Quad Settings")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float offsetBelowCeiling = 0.15f;
    [SerializeField] private bool overrideCeilingScale = true;
/*
    [Header("Room Particle Settings")]
    [SerializeField] private GameObject roomParticlePrefab;
    [SerializeField] private float offsetBelowRoomCeiling = 0.2f;
    [SerializeField] private bool spawnRoomParticle = true;
*/
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

                if ((anchor.Label & MRUKAnchor.SceneLabels.CEILING) != 0)
                {
                    SpawnBelowCeiling(anchor);
                }
            }

           /* // 🧨 Spawn particles at center-top of this room
            if (spawnRoomParticle)
            {
                SpawnParticleInRoomCenter(room);
            }*/
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
            obj.transform.localScale = new Vector3(10f, 10f, 10f); // Visible size
        }

        obj.name = "CeilingQuad_" + anchor.name;

        var videoPlayer = obj.GetComponentInChildren<UnityEngine.Video.VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.targetMaterialRenderer = obj.GetComponent<Renderer>();
            videoPlayer.Play();
        }
    }

   /* private void SpawnParticleInRoomCenter(MRUKRoom room)
    {
        Vector3 centerXZ = Vector3.zero;
        float ceilingY = float.MinValue;
        int count = 0;

        foreach (var anchor in room.Anchors)
        {
            if (!anchor.VolumeBounds.HasValue) continue;

            Vector3 worldCenter = anchor.transform.TransformPoint(anchor.VolumeBounds.Value.center);
            centerXZ += new Vector3(worldCenter.x, 0, worldCenter.z);
            count++;

            if ((anchor.Label & MRUKAnchor.SceneLabels.CEILING) != 0 && worldCenter.y > ceilingY)
            {
                ceilingY = worldCenter.y;
            }
        }

        if (count == 0)
        {
            Debug.LogWarning("⚠️ No valid anchors for particle placement.");
            return;
        }

        centerXZ /= count;

        Vector3 spawnPos = new Vector3(centerXZ.x, ceilingY - offsetBelowRoomCeiling, centerXZ.z);
        GameObject fx = Instantiate(roomParticlePrefab, spawnPos, Quaternion.identity);
        fx.name = $"RoomParticle_{room.name}";

        Debug.Log($"✨ Particle effect spawned at: {spawnPos} in room: {room.name}");
    } */
   
}
