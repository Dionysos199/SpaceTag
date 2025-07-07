using Firebase.Firestore;
using Meta.XR.MRUtilityKit;
using UnityEngine;
public enum ObjectsToSpawn
{
    painting,
    vase,
    cat,
}

public class SpawnDynamically : MonoBehaviour
{

    public MRUKAnchor.SceneLabels selectedLabels = MRUKAnchor.SceneLabels.BED;
    public GameObject[] gift; // Match enum order in Inspector
    FirebaseFirestore db;
    //ListenerRegistration registration;
    string lastPrefabID = null;

    private void Start()
    {
        //db = FirebaseFirestore.DefaultInstance;
        //registration = db.Collection("Splace").Document("Splace").Listen(snapshot =>
        //{
        //    if (snapshot.Exists)
        //    {
        //        SaveData SaveData = snapshot.ConvertTo<SaveData>();
        //        if (SaveData.PrefabID != lastPrefabID)
        //        {
        //            lastPrefabID = SaveData.PrefabID;
        //            OnPrefabIDUpdated(SaveData.PrefabID, SaveData.PlaceID);
        //        }
        //    }
        //});
    }
    private void OnDestroy()
    {
        //registration?.Stop();
    }

    public void OnPrefabIDUpdated(string newPrefabID, string placeLabel)
    {
        SpawnPrefab(newPrefabID, placeLabel);
    }

    private void SpawnPrefab(string prefabID, string placeLabel)
    {

        Debug.Log($"PrefabID changed to: {prefabID}");
        Debug.Log($"placeID changed to: {placeLabel}");
        if (System.Enum.TryParse(prefabID, true, out ObjectsToSpawn parsedEnum))
        {
            int index = (int)parsedEnum;
            Debug.Log("index "+ index + "parsedEnum "+ parsedEnum);
            if (index >= 0 && index < gift.Length)
            {
                GameObject prefabToSpawn = gift[index];

                // Try to parse the place label (like "BED", "DESK", etc.)
                MRUKAnchor.SceneLabels parsedLabel = selectedLabels; // fallback to current
                if (!string.IsNullOrEmpty(placeLabel) && System.Enum.TryParse(placeLabel, true, out MRUKAnchor.SceneLabels label))
                {
                    parsedLabel = label;
                    Debug.Log("place Label is"+ parsedLabel);
                }
                else
                {
                    Debug.LogWarning($"Invalid or missing place label: {placeLabel}, using fallback: {selectedLabels}");
                }
                var findSpawnPos = gameObject.AddComponent<FindSpawnPositions>();
                findSpawnPos.SpawnObject = prefabToSpawn;
                findSpawnPos.SpawnLocations = FindSpawnPositions.SpawnLocation.OnTopOfSurfaces;
                findSpawnPos.SpawnAmount = 1;
                findSpawnPos.Labels = parsedLabel;
            }
        }
        else
        {
            Debug.Log($"Failed to parse '{prefabID}'");
        }
    }

}
