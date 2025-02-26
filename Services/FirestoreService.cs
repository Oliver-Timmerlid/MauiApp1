

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



}