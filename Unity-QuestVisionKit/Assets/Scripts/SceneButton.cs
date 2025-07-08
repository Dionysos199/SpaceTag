using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    [SerializeField] private SendMessageHandler sendMessageHandler;

    private void OnButtonClicked()
    {
        sendMessageHandler.SceneName = sceneName;

        Debug.Log($"Button clicked! Scene: {sceneName}");
    }
}