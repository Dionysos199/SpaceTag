using Firebase.Firestore;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendMessageHandler : MonoBehaviour
{
    private FirebaseFirestore firestore;
    private string senderUsername;  // From SaveSystem
    private string receiverUsername; // Selected receiver

    [Header("Inputs")]
    [SerializeField] private TMP_InputField placeIDInput;
    [SerializeField] private Toggle aiPromptToggleInput;

    [Header("Public Variables (Assigned Dynamically)")]
    public string SceneName { get; set; }  // Assigned by button
    public string PrefabID { get; set; }   // Assigned by button

    [Header("UI Feedback")]
    [SerializeField] private TMPro.TextMeshProUGUI feedbackText;

    private void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;
    }

    public void SetSenderUsername(string username)
    {
        senderUsername = username;
    }

    public void SetReceiverUsername(string username)
    {
        receiverUsername = username;
    }

    // Send message for NightSky Scene
    public async void SendNightSkyMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = (senderUsername != receiverUsername),
            SceneName = SceneName  // Assigned by button
        };

        await SendMessageToFirestore(message);
    }

    // Send message for MothersDay
    public async void SendMothersDayMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            PrefabID = PrefabID,  // Assigned dynamically
            PlaceID = placeIDInput.text,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = (senderUsername != receiverUsername),
            SceneName = SceneName
        };

        await SendMessageToFirestore(message);
    }

    // Send message for Birthday Scene (also loads scene)
    public async void SendBirthdayMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            PrefabID = PrefabID,
            PlaceID = placeIDInput.text,
            AIPrompt = aiPromptToggleInput.isOn,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = (senderUsername != receiverUsername),
            SceneName = SceneName
        };

        await SendMessageToFirestore(message);
    }

    // Send message for Perrot
    public async void SendPerrotMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            PrefabID = PrefabID,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = (senderUsername != receiverUsername),
            SceneName = SceneName
        };

        await SendMessageToFirestore(message);
    }

    // Send message for Spatial3D
    public async void SendSpatial3DMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            PrefabID = PrefabID,
            PlaceID = placeIDInput.text,
            AIPrompt = aiPromptToggleInput.isOn,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = (senderUsername != receiverUsername),
            SceneName = SceneName
        };

        await SendMessageToFirestore(message);
    }

    private async Task SendMessageToFirestore(SaveData message)
    {
        try
        {
            await firestore.Document($"save_data/{receiverUsername}").SetAsync(message);
            feedbackText.text = $"Message sent to {receiverUsername}!";
        }
        catch (System.Exception e)
        {
            feedbackText.text = $"Failed to send message: {e.Message}";
        }
    }

    private bool ValidateUsers()
    {
        if (string.IsNullOrEmpty(receiverUsername))
        {
            feedbackText.text = "No receiver selected.";
            return false;
        }
        if (string.IsNullOrEmpty(senderUsername))
        {
            feedbackText.text = "Sender username not set.";
            return false;
        }
        return true;
    }
}
