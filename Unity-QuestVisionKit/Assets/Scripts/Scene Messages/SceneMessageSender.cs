using Firebase.Firestore;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public abstract class SceneMessageSender : MonoBehaviour
{
    protected FirebaseFirestore firestore;
    protected string senderUsername;
    protected string receiverUsername;

    [Header("UI Feedback")]
    [SerializeField] protected TMP_Text feedbackText;

    protected virtual void Awake()
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

    protected bool ValidateUsers()
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

    protected async Task SendMessageToFirestore(SaveData message)
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

    public abstract void SendMessage();  // Implement in derived classes
}
