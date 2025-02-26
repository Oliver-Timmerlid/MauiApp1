//using Firebase.Database;
//using Firebase.Database.Query;
//using Google.Cloud.Firestore;
//using System.Threading.Tasks;

//namespace MauiApp1.Services;

//internal class Firebase
//{
//    private readonly FirestoreDb _firebaseClient;

//    public Firebase()
//    {
//        _firebaseClient = FirestoreDb.Create("your-project-id");
//    }

//    public async Task<bool> IsAndroidIdFoundAsync(string androidId)
//    {
//        var result = await _firebaseClient
//            .Collection("androidIds")
//            .WhereEqualTo(FieldPath.DocumentId, androidId)
//            .GetSnapshotAsync();

//        return result.Count > 0;
//    }
//}
