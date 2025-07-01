using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private FirebaseFirestore firestore;

    public void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }
    public void saveToCloud()
    {
        SaveData saveData = new();
        firestore.Document($"save_data/{saveData.UserName}").SetAsync(saveData);
    }
    public void loadFromCloud() 
    {
        SaveData saveData = new();
        firestore.Document($"save_data/0").GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.Result.Exists)
            {
                var data = task.Result.ConvertTo<SaveData>();
                Debug.Log($"username :{data.UserName} "+ $" PrefabID :{data.prefabID}"+ $" PlaceID :{data.placeID}");
            }
        });
    }
}
