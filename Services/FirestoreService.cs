using Google.Cloud.Firestore;
using MauiApp1.Models;

public class FirestoreService
{

    private FirestoreDb db;

    // Initializes the Firestore database if not already set up
    private async Task SetupFirestore()
    {
        if (db == null)
        {
            //IMPORTANT! READ THIS!
            //This is for adding firebase database to the application. If creating a new firebase database, this needs to be changed to the new firebase link
            var stream = await FileSystem.OpenAppPackageFileAsync("ulum-dalska-firebase-adminsdk-fbsvc-a4f09f4f41.json");
            var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();
    
    
            // Create a new Firestor database instance with project settings
            db = new FirestoreDbBuilder
            {
                ProjectId = "ulum-dalska",
                ConverterRegistry = new ConverterRegistry 
                {                  
                },
                JsonCredentials = contents
            }.Build();
        }
    }
    // Inserts a new user into the Firestore 'Users' collection
    public async Task InsertUser(User user) 
    {
        await SetupFirestore();
        await db.Collection("Users").AddAsync(user);
    } 

    // Check if user in database
    public async Task<bool> CheckUser(string id) 
    {
        await SetupFirestore();
        var query = db.Collection("Users").WhereEqualTo("AndroidId", id);
        var snapshot = await query.GetSnapshotAsync();
        return snapshot.Documents.Count > 0;
    }

    // Get User from database
    public async Task<User> GetUser(string uuid)
    {
        await SetupFirestore();
        var query = db.Collection("Users").WhereEqualTo("Uuid", uuid);
        var snapshot = await query.GetSnapshotAsync();

        if (snapshot.Documents.Count > 0)
        {
            var document = snapshot.Documents.First();
            return document.ConvertTo<User>();
        }
        return null;
    }

    // Update user in database
    public async Task UpdateUser(User user)
    {
        await SetupFirestore();
        var query = db.Collection("Users").WhereEqualTo("Uuid", user.Uuid);
        var snapshot = await query.GetSnapshotAsync();
        if (snapshot.Documents.Count > 0)
        {
            var document = snapshot.Documents.First();
            await document.Reference.SetAsync(user);
        }
    }    
}