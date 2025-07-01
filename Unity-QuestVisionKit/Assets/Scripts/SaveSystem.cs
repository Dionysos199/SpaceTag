using Firebase.Firestore;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class SaveSystem : MonoBehaviour
{
    private FirebaseFirestore firestore;

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TextMeshProUGUI feedbackText;

    private string currentUsername;

    void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }

    // Called by Register Button
    public void OnRegisterButton()
    {
        string enteredUsername = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(enteredUsername))
        {
            feedbackText.text = "Username cannot be empty.";
            return;
        }

        CheckIfUserExists(enteredUsername).ContinueWith(task =>
        {
            if (task.Result)
            {
                feedbackText.text = "Username already exists.";
            }
            else
            {
                currentUsername = enteredUsername;
                SaveData newUser = new(currentUsername);
                firestore.Document($"save_data/{currentUsername}").SetAsync(newUser);
                feedbackText.text = "User registered and data saved!";
            }
        });
    }

    // Called by Login Button
    public void OnLoginButton()
    {
        string enteredUsername = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(enteredUsername))
        {
            feedbackText.text = "Enter your username.";
            return;
        }

        firestore.Document($"save_data/{enteredUsername}").GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.Result.Exists)
            {
                var data = task.Result.ConvertTo<SaveData>();
                currentUsername = data.UserName;

                Debug.Log($"Welcome {data.UserName} | PrefabID: {data.PrefabID}, PlaceID: {data.PlaceID}");
                feedbackText.text = $"Welcome back, {data.UserName}!";
            }
            else
            {
                feedbackText.text = "User not found. Please register first.";
            }
        });
    }

    // Saves updated prefab/place data
    public void SaveProgress(string prefabID, string placeID)
    {
        if (string.IsNullOrEmpty(currentUsername))
        {
            feedbackText.text = "No user logged in.";
            return;
        }

        SaveData updatedData = new(currentUsername, prefabID, placeID);
        firestore.Document($"save_data/{currentUsername}").SetAsync(updatedData);
        feedbackText.text = "Progress saved!";
    }

    // Check if username already exists
    private async Task<bool> CheckIfUserExists(string username)
    {
        DocumentSnapshot snapshot = await firestore.Document($"save_data/{username}").GetSnapshotAsync();
        return snapshot.Exists;
    }
}
