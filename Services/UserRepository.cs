using CryptoTradingDesktopApp.Api.Models;
using Google.Cloud.Firestore;
using System;
using System.Threading.Tasks;

namespace CryptoTradingDesktopApp.Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly FirestoreDb _firestoreDb;

        public UserRepository(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid userId)
        {
            DocumentReference docRef = _firestoreDb.Collection("Users").Document(userId.ToString());
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<UserModel>();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public async Task AddUserAsync(UserModel user)
        {
            DocumentReference docRef = _firestoreDb.Collection("Users").Document(user.UserId.ToString());
            await docRef.SetAsync(user);
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            DocumentReference docRef = _firestoreDb.Collection("Users").Document(user.UserId.ToString());
            await docRef.SetAsync(user, SetOptions.Overwrite);
        }
    }
}