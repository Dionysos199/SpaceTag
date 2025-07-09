using TMPro;
using UnityEngine;

public class MothersDayMessageSender : SceneMessageSender
{
    [SerializeField] private TMP_InputField placeIDInput;
    [SerializeField] private string prefabID;
    [SerializeField] private string sceneName = "MothersDay";

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
            PlaceID = placeIDInput.text
        };

        await SendMessageToFirestore(message);
    }
}
