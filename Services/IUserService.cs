using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface IUserService
    {
        Task<Api.Models.RegistrationResult> RegisterUserAsync(UserRegistrationModel model);
        Task<string?> LoginUserAsync(UserLoginModel model); // Returns a token on successful login
    }
}
