using Firebase.Database;
// using Firebase.Database.Query;
using Google.Cloud.Firestore;
using System.Threading.Tasks;

namespace MauiApp1.Services;

public class FirestoreService
{
    private FirestoreDb db;

    private async Task SetupFirestore()
    { 
        if (db == null)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("ulum-dalska-firebase-adminsdk-fbsvc-72ece19c2f.json");
            var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();

            db = new FirestoreDbBuilder
            {
                ProjectId = "ulum-dalska",

                ConverterRegistry = new ConverterRegistry
                {
                    //new DateTimeToTimestampConverter(),
                },
                JsonCredentials = contents
            }.Build();
        }
    }
    
}
