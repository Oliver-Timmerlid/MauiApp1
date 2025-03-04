using Google.Cloud.Firestore;

namespace MauiApp1.Models;

[FirestoreData]
public class User{

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string AndroidId { get; set; }

    [FirestoreProperty]
    public string Uuid { get; set; }

    public User()
    {
        
    }
    public User(string name, string androidId, string uuid)
    {
        Name = name;
        AndroidId = androidId;
        Uuid = uuid;
    }

    
}