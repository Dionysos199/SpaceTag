using UnityEngine;

public class PerrotMessageSender : SceneMessageSender
{
    [SerializeField] private string prefabID;
    [SerializeField] private string sceneName = "Perrot";

    public override async void SendMessage()
    {
        if (!ValidateUsers()) return;

        SaveData message = new SaveData
        {
            UserName = receiverUsername,
            LastModifiedBy = senderUsername,
            IsModifiedByAnotherUser = senderUsername != receiverUsername,
            SceneName = sceneName,
            PrefabID = prefabID
        };

        await SendMessageToFirestore(message);
    }
}
