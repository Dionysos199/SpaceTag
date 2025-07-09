using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirestoreUpdater : MonoBehaviour
{
    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        UpdateDocument("save_data");
    }

    public void UpdateDocument(string documentId)
    {
        SaveData newData = new SaveData("Imad")
        {
            LastModifiedBy = "SystemUpdate",
            IsModifiedByAnotherUser = false,
            personalizedMessage = "New Message!",
            AIPrompt = true,
            AIPrompt_Text = "Hello AI",
            SceneName = "SampleScene"
        };

        db.Collection("Imad").Document(documentId)
          .SetAsync(newData)
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsCompleted)
              {
                  Debug.Log("Document successfully updated with new fields!");
              }
              else
              {
                  Debug.LogError("Failed to update document: " + task.Exception);
              }
          });
    }
}
