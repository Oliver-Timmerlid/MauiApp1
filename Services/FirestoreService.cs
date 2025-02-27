

using Google.Cloud.Firestore;

public class FirestoreService
{

    private FirestoreDb db;

    private async Task SetupFirestore()
    {
        if (db == null)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("ulum-dalska-firebase-adminsdk-fbsvc-a4f09f4f41.json");
            var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();
    
    

            db = new FirestoreDbBuilder
            {
                ProjectId = "ulum-dalska",
                ConverterRegistry = new ConverterRegistry 
                {
                    //new DateTimeToTimeConvert()
                },
                JsonCredentials = contents
            }.Build();
        }
    }

    public async Task InsertUser(User user) 
    {
        await SetupFirestore();
        await db.Collection("Users").AddAsync(user);
    } 

    public async Task<bool> CheckUser(string id) 
    {
        await SetupFirestore();
        var query = db.Collection("Users").WhereEqualTo("AndroidId", id);
        var snapshot = await query.GetSnapshotAsync();
        return snapshot.Documents.Count > 0;
    }

    //public async Task<User> GetUsers(string uuId)
    //{
    //    await SetupFirestore();
    //    var query = db.Collection("Users").WhereEqualTo("Uuid", uuId);

    //}

    // copilot
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

    // update user
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