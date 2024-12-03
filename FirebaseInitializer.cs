using Google.Cloud.Firestore;

namespace CryptoTradingDesktopApp.Api
{
    public static class FirestoreInitializer
    {
        public static async Task InitializeCollections(FirestoreDb db)
        {
            string[] collections = { "MarketData", "OrderBook", "UserProfiles" };

            foreach (var collection in collections)
            {
                var colRef = db.Collection(collection);
                var snapshot = await colRef.Limit(1).GetSnapshotAsync();
                if (!snapshot.Any())
                {
                    await colRef.AddAsync(new Dictionary<string, object>
                    {
                        { "InitializedAt", Timestamp.GetCurrentTimestamp() }
                    });
                    Console.WriteLine($"Initialized {collection} collection");
                }
            }
        }
    }
}