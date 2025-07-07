using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using Firebase;
using Firebase.Storage;
using Firebase.Extensions;
public class ImageLoader : MonoBehaviour
{
    private RawImage rawImage;
    private FirebaseStorage storage;
    private StorageReference storageReference;

    void Start()
    {
        rawImage = GetComponent<RawImage>();

        
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://spacetag-ca868.firebasestorage.app");
        StorageReference image = storageReference.Child("Capture.PNG");
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted &&!task.IsCanceled)
            {
                StartCoroutine(LoadImage(task.Result.ToString()));
            }
            else
            {
                Debug.Log(task.Exception);
            }
        }
        ) ;
    }

    public IEnumerator LoadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Image download failed: " + request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            rawImage.texture = texture;
        }
    }
}
