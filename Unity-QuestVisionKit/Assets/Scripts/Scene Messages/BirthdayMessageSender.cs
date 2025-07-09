using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BirthdayMessageSender : SceneMessageSender
{
    [SerializeField] private TMP_InputField placeIDInput;
    [SerializeField] private Toggle aiPromptToggle;
    [SerializeField] private string prefabID;
    [SerializeField] private string sceneName = "Birthday";

    public override async void SendMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = senderUsername != receiverUsername,
            SceneName = sceneName,
            PrefabID = prefabID,
            PlaceID = placeIDInput.text,
            AIPrompt = aiPromptToggle.isOn
        };

        await SendMessageToFirestore(message);
    }
}
