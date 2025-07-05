using Meta.XR.MRUtilityKit;
using UnityEngine;

public class ForestSky : MonoBehaviour
{
     [Header("Ceiling Quad Settings")]
    [SerializeField] private GameObject ceilingPrefab;
    [SerializeField] private float offsetBelowCeiling = 0.15f;
    [SerializeField] private bool overrideCeilingScale = true;

    
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
            obj.transform.localScale = new Vector3(10f, 10f, 10f); // Visible size
        }

       /* var rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.magenta;
        }*/

        obj.name = "CeilingQuad_" + anchor.name;
        
        var videoPlayer = obj.GetComponentInChildren<UnityEngine.Video.VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.targetMaterialRenderer = obj.GetComponent<Renderer>();
            videoPlayer.Play();
        }
    }

   
}
