using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageHandler
{
    private readonly FirebaseFirestore firestore;
    private readonly string currentUsername;
    private readonly SpawnDynamically spawnDynamically;
    private readonly GameObject notificationButton;
    private readonly TMPro.TextMeshProUGUI feedbackText;

    public MessageHandler(string username, SpawnDynamically spawn, GameObject notifButton, TMPro.TextMeshProUGUI feedback)
    {
        firestore = FirebaseFirestore.DefaultInstance;
        currentUsername = username;
        spawnDynamically = spawn;
        notificationButton = notifButton;
        feedbackText = feedback;
    }

    public async void OnNotificationButtonPressed()
    {
        if (string.IsNullOrEmpty(currentUsername)) return;

        DocumentReference docRef = firestore.Document($"save_data/{currentUsername}");
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            SaveData data = snapshot.ConvertTo<SaveData>();
            data.IsModifiedByAnotherUser = false;
            await docRef.SetAsync(data);

            notificationButton.SetActive(false);
            feedbackText.text = "Changes acknowledged.";

            HandleMessage(data);
        }
    }

    private void HandleMessage(SaveData data)
    {
        string messageType = data.SceneName;

        switch (messageType)
        {
            case "nightSky":
                Debug.Log($"[NightSky] Scene: {data.SceneName}, Receiver: {data.UserName}");
                SceneManager.LoadScene("NightSky");
                break;

            case "MothersDay":
                Debug.Log($"[MothersDay] Prefab: {data.PrefabID}, Place: {data.PlaceID}");
                spawnDynamically.OnPrefabIDUpdated(data.PrefabID, data.PlaceID);
                break;

            case "Birthday":
                Debug.Log($"[Birthday] Scene: {data.SceneName}, Receiver: {data.UserName}");
                SceneManager.LoadScene("Birthday");
                if (data.AIPrompt)
                {
                    // Handle AI prompt here
                }
                break;

            case "perrot":
                Debug.Log($"[Perrot] Prefab: {data.PrefabID}");
                spawnDynamically.OnPrefabIDUpdated(data.PrefabID, "");
                break;

            case "spatial3D":
                if (data.AIPrompt)  
                Debug.Log($"[Spatial3D] Prefab: {data.PrefabID}, Place: {data.PlaceID}, AI Prompt: {data.AIPrompt}");
                spawnDynamically.OnPrefabIDUpdated(data.PrefabID, data.PlaceID);
                if (data.AIPrompt)
                {
                    // Handle AI prompt here too
                }
                break;

            default:
                Debug.LogWarning($"Unknown message type: {messageType}");
                break;
        }
    }
}
