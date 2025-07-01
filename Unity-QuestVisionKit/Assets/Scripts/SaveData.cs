using Firebase.Firestore;

[FirestoreData]
public class SaveData
{
    [FirestoreProperty]
    public string UserName { get; set; } = "Imad";

    [FirestoreProperty]
    public string PrefabID { get; set; } = "Cat";

    [FirestoreProperty]
    public string PlaceID { get; set; } = "Couch";

    public SaveData() { }

    public SaveData(string userName, string prefabID = "Cat", string placeID = "Couch")
    {
        UserName = userName;
        PrefabID = prefabID;
        PlaceID = placeID;
    }
}
