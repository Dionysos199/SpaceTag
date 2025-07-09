using Firebase.Firestore;
using TMPro;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class SaveSystem : MonoBehaviour
{
    private FirebaseFirestore firestore;

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private GameObject notificationButton;
    [SerializeField] private TextMeshProUGUI messageReceivedFrom;
    [SerializeField] private SceneMessageSender sceneMessageSender;
    private string currentUsername;

    [SerializeField] private SpawnDynamically spawnDynamically;

    private ListenerRegistration currentListener;

    void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }

    public async void OnRegisterButton()
    {
        string enteredUsername = usernameInput.text.Trim();
        if (string.IsNullOrEmpty(enteredUsername))
        {
            feedbackText.text = "Username cannot be empty.";
            return;
        }

        bool exists = await CheckIfUserExists(enteredUsername);
        if (exists)
        {
            feedbackText.text = "Username already exists.";
        }
        else
        {
            currentUsername = enteredUsername;
            SaveData newUser = new(currentUsername);
            await firestore.Document($"save_data/{currentUsername}").SetAsync(newUser);
            feedbackText.text = "User registered!";
        }
    }

    [SerializeField] TextMeshProUGUI DebugText1;


    [SerializeField] private TMP_Dropdown userDropdown;
    public async void OnLoginButton()
    {

        //string enteredUsername = usernameInput.text.Trim();
        string enteredUsername = userDropdown.options[userDropdown.value].text;


        DebugText1.text = enteredUsername;

        DebugText1.text = ($"sceneMessageSender is null? {sceneMessageSender == null}");
        sceneMessageSender.SetSenderUsername(currentUsername); // This may throw
        if (string.IsNullOrEmpty(enteredUsername))
        {
            feedbackText.text = "Enter your username.";
            return;
        }

        try
        {
            DocumentSnapshot snapshot = await firestore.Document($"save_data/{enteredUsername}").GetSnapshotAsync();

            if (snapshot.Exists)
            {
                SaveData data = snapshot.ConvertTo<SaveData>();
                currentUsername = data.UserName; 
                feedbackText.text = $"Welcome back, {data.UserName}!";

                ListenToUserData(currentUsername);
            }
            else
            {
                feedbackText.text = "User not found. Please register first.";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Login failed: " + ex.Message);
            feedbackText.text = "Login failed: " + ex.Message;
        }
    }

    private void ListenToUserData(string username)
    {
        currentListener?.Stop();

        currentListener = firestore.Document($"save_data/{username}").Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                SaveData data = snapshot.ConvertTo<SaveData>();
                Debug.Log($"[Realtime Sync] Data changed: {data.PrefabID}, {data.PlaceID}, {data.SceneName}");
                if (data.IsModifiedByAnotherUser)
                {
                    messageReceivedFrom.text = $"You received a message from {data.LastModifiedBy}";
                }

                notificationButton.gameObject.SetActive(data.IsModifiedByAnotherUser);
            }
        });
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

            notificationButton.gameObject.SetActive(false);
            spawnDynamically.OnPrefabIDUpdated(data.PrefabID, data.PlaceID);
            feedbackText.text = "Changes acknowledged.";
        }
    }

    private async Task<bool> CheckIfUserExists(string username)
    {
        DocumentSnapshot snapshot = await firestore.Document($"save_data/{username}").GetSnapshotAsync();
        return snapshot.Exists;
    }

    private void OnDestroy()
    {
        currentListener?.Stop();
    }

    public string GetUsername()
    {
        return currentUsername;
    }
}
