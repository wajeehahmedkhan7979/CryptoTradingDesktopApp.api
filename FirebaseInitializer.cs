using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.IO;

namespace CryptoTradingDesktopApp.Api
{
    
}
public class FirebaseInitializer
{
    public static void InitializeFirebase()
    {
        // Load the credentials from the JSON string
        string jsonCredentials = @"
        {
            ""type"": ""service_account"",
            ""project_id"": ""cryptotradingdesktopapp"",
            ""private_key_id"": ""8898361c4ff3d994dad9cf5e7d640b12b83362e5"",
            ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADAN...YOUR_KEY...I=\n-----END PRIVATE KEY-----\n"",
            ""client_email"": ""firebase-adminsdk-bm2zk@cryptotradingdesktopapp.iam.gserviceaccount.com"",
            ""client_id"": ""106979270755081428230"",
            ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
            ""token_uri"": ""https://oauth2.googleapis.com/token"",
            ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
            ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-bm2zk%40cryptotradingdesktopapp.iam.gserviceaccount.com""
        }";

        using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonCredentials)))
        {
            // Initialize the Firebase Admin SDK
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromStream(stream)
            });

            // Create Firestore instance
            FirestoreDb firestore = FirestoreDb.Create("cryptotradingdesktopapp");

            Console.WriteLine("Firebase and Firestore Initialized");
        }
    }
}
