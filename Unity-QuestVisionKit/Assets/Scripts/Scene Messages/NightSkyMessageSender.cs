using TMPro;
using UnityEngine;

public class NightSkyMessageSender : SceneMessageSender
{
    [SerializeField] private TMP_InputField personalisedMessageInput;
    [SerializeField] private string sceneName = "NightSky";

    public override async void SendMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = senderUsername != receiverUsername,
            SceneName = sceneName,
            personalizedMessage = personalisedMessageInput.text
        };

        await SendMessageToFirestore(message);
    }
}
