using Google.Cloud.Firestore;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreService(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        public async Task AddDocumentAsync<T>(string collectionName, string documentId, T data)
        {
            var collection = _firestoreDb.Collection(collectionName);
            var document = collection.Document(documentId);
            await document.SetAsync(data);
        }

        public async Task<T?> GetDocumentAsync<T>(string collectionName, string documentId) where T : class
        {
            var document = _firestoreDb.Collection(collectionName).Document(documentId);
            var snapshot = await document.GetSnapshotAsync();
            return snapshot.Exists ? snapshot.ConvertTo<T>() : null;
        }

        public async Task DeleteDocumentAsync(string collectionName, string documentId)
        {
            var document = _firestoreDb.Collection(collectionName).Document(documentId);
            await document.DeleteAsync();
        }
    }
}
