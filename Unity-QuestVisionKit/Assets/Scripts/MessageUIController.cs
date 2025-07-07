using UnityEngine;

public class MessageUIController : MonoBehaviour
{
    [SerializeField] private SendMessageHandler sendMessageHandler;

    public void OnBirthdayButtonClicked()
    {
        sendMessageHandler.SceneName = "Birthday";
    }

    public void OnNightSkyButtonClicked()
    {
        sendMessageHandler.SceneName = "MothersDay";
    }

    public void OnMothersDayButtonClicked()
    {
        
    }
}
