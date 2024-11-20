using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CryptoTradingDesktopApp.Api
{
    public static class FirebaseConfig
    {
        public static void InitializeFirebase()
        {
            var firebaseConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "serviceAccountKey.json");

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(firebaseConfigPath),
            });
        }
    }
}
