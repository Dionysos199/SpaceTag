using Firebase.Firestore;
using UnityEngine;
[FirestoreData]
public class SaveData
{
    private string userName = "Imad";
    private string PrefabID = "Cat";
    private string PlaceID = "Couch";
    [FirestoreProperty]
    public string UserName
    {
        get => userName;
        set => userName = value;
    }
    [FirestoreProperty]
    public string prefabID
    {
        get => PrefabID;
        set => PrefabID = value;
    }
    [FirestoreProperty]
    public string placeID
    {
        get => PlaceID;
        set => PlaceID = value;
    }

}
