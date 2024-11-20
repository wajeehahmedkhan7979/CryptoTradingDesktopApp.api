using Google.Cloud.Firestore;
using CryptoTradingDesktopApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTradingDesktopApp.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreService(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }

        public async Task<bool> AddOrderAsync(OrderModel order)
        {
            try
            {
                CollectionReference ordersRef = _firestoreDb.Collection("orders");
                await ordersRef.AddAsync(order);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<OrderModel>> GetOrdersAsync(OrderType type)
        {
            Query query = _firestoreDb.Collection("orders").WhereEqualTo("Type", type);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            List<OrderModel> orders = new List<OrderModel>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                orders.Add(document.ConvertTo<OrderModel>());
            }

            return orders;
        }

        public async Task<bool> UpdateWalletBalanceAsync(Guid userId, decimal newBalance)
        {
            try
            {
                DocumentReference walletRef = _firestoreDb.Collection("wallets").Document(userId.ToString());
                await walletRef.SetAsync(new { Balance = newBalance }, SetOptions.MergeAll);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<WalletModel?> GetWalletAsync(Guid userId)
        {
            DocumentReference walletRef = _firestoreDb.Collection("wallets").Document(userId.ToString());
            DocumentSnapshot snapshot = await walletRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<WalletModel>();
            }

            return null;
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            Query query = _firestoreDb.Collection("users").WhereEqualTo("Email", email);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count > 0)
            {
                return snapshot.Documents[0].ConvertTo<UserModel>();
            }

            return null;
        }

        public async Task<bool> AddUserAsync(UserModel user)
        {
            try
            {
                DocumentReference userRef = _firestoreDb.Collection("users").Document(user.UserId.ToString());
                await userRef.SetAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveOrderAsync(Guid orderId)
        {
            try
            {
                DocumentReference orderRef = _firestoreDb.Collection("orders").Document(orderId.ToString());
                await orderRef.DeleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrderAsync(OrderModel order)
        {
            try
            {
                DocumentReference orderRef = _firestoreDb.Collection("orders").Document(order.Id.ToString());
                await orderRef.SetAsync(order, SetOptions.MergeAll);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}