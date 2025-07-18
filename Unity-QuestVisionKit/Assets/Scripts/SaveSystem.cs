using Firebase.Firestore;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

public class SaveSystem : MonoBehaviour
{
    private FirebaseFirestore firestore;

    [Header("UI Elements")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [SerializeField] private TMP_Dropdown userDropdown;

    private List<string> userList = new();
    private string selectedReceiver;
    private string currentUsername;
    [SerializeField] GameObject notificationButton;
    [SerializeField] TextMeshProUGUI messageReceivedFrom;
    void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;

        FetchUserList(); // Populate dropdown on load
    }

    public void FetchUserList()
    {
        firestore.Collection("save_data").GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                QuerySnapshot snapshot = task.Result;
                userList.Clear();

                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    userList.Add(doc.Id); // Use document ID as username
                }

                PopulateDropdown();
            }
            else
            {
                feedbackText.text = "Failed to load users.";
            }
        });
    }
    [SerializeField] private DropdownPopulator usersDropdownPopulator;
    void PopulateUserDropdown()
    {
        Debug.Log("[SaveSystem] Populating user dropdown with fetched users.");
        usersDropdownPopulator.Populate(userList);
    }

    void PopulateDropdown()
    {
        userDropdown.ClearOptions();
        userDropdown.AddOptions(userList);
        if (userList.Count > 0)
        {
            selectedReceiver = userList[0];
        }

        userDropdown.onValueChanged.AddListener(index =>
        {
            selectedReceiver = userList[index];
            Debug.Log("selected Username "+selectedReceiver);
        });
    }
    public void OnUserNameSelected()
    {
        selectedReceiver = userDropdown.options[userDropdown.value].text;
        Debug.Log("Selected Username: " + selectedReceiver);
    }


    [SerializeField] private TMP_Dropdown prefabID_DD;
    [SerializeField] private TMP_Dropdown placeID_DD;
    private string prefabID;
    private string placeID;

    public async void SendMessage()
    {
        if (placeID_DD.options.Count == 0 || prefabID_DD.options.Count == 0)
        {
            feedbackText.text = "Dropdowns are not populated.";
            return;
        }

        placeID = placeID_DD.options[placeID_DD.value].text;
        prefabID = prefabID_DD.options[prefabID_DD.value].text;

        if (string.IsNullOrEmpty(selectedReceiver))
        {
            feedbackText.text = "No user selected.";
            return;
        }

        SaveData updatedData = new SaveData
        {
            UserName = selectedReceiver,
            PrefabID = prefabID,
            PlaceID = placeID,
            LastModifiedBy = currentUsername,
            IsModifiedByAnotherUser = (currentUsername != selectedReceiver)
        };

        try
        {
            await firestore.Document($"save_data/{selectedReceiver}").SetAsync(updatedData);
            feedbackText.text = $"Progress saved for {selectedReceiver}!";
        }
        catch (System.Exception e)
        {
            feedbackText.text = $"Save failed: {e.Message}";
        }
    }

    // Called by Register Button
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
            feedbackText.text = "User registered and data saved!";
            FetchUserList(); // Update dropdown after new user
        }
    }


    public async void OnLoginButton()
    {
        string enteredUsername = usernameInput.text.Trim();

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

                Debug.Log($"Welcome {data.UserName} | PrefabID: {data.PrefabID}, PlaceID: {data.PlaceID}");
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
            feedbackText.text = "Login error.";
        }
    }



    private ListenerRegistration currentListener;

    private void ListenToUserData(string username)
    {
        // Clean up any previous listener
        currentListener?.Stop();

        // Start new listener
        currentListener = firestore.Document($"save_data/{username}").Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                SaveData data = snapshot.ConvertTo<SaveData>();
                Debug.Log($"[Realtime Sync] {username} data changed: {data.PrefabID}, {data.PlaceID}");
                if (data.IsModifiedByAnotherUser)
                {
                     messageReceivedFrom.text = ($"You received a message from {data.LastModifiedBy}");
                }
                // Show notification UI if modified by another user
                notificationButton.gameObject.SetActive(data.IsModifiedByAnotherUser);

                // Update UI or game state with new data here
            }
        });
    }

    private void OnDestroy()
    {
        currentListener?.Stop();
    }
    [SerializeField]    SpawnDynamically spawnDynamically;
    public async void OnNotificationButtonPressed()
    {
        if (string.IsNullOrEmpty(currentUsername)) return;

        // Fetch current data first
        DocumentReference docRef = firestore.Document($"save_data/{currentUsername}");
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            SaveData data = snapshot.ConvertTo<SaveData>();
            data.IsModifiedByAnotherUser = false;

            // Save back updated data
            await docRef.SetAsync(data);

            notificationButton.gameObject.SetActive(false);
            spawnDynamically.OnPrefabIDUpdated(data.PrefabID, data.PlaceID);
            // Trigger any other logic for user acknowledging the update here
            feedbackText.text = "Changes acknowledged.";
        }
    }
    public string GetUsername()
    {
        return currentUsername;
    }


    // Check if username already exists
    private async Task<bool> CheckIfUserExists(string username)
    {
        DocumentSnapshot snapshot = await firestore.Document($"save_data/{username}").GetSnapshotAsync();
        return snapshot.Exists;
    }
}
