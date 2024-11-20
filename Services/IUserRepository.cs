using System;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Data
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserByIdAsync(Guid userId);
        Task AddUserAsync(UserModel user);
        Task UpdateUserAsync(UserModel user);
    }
}