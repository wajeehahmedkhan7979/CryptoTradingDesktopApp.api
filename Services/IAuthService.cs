// CryptoTradingDesktopApp.Api.Services/IAuthService.cs
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(CryptoTradingDesktopApp.Api.Models.UserLoginModel model);
        Task<AuthResult> RegisterAsync(CryptoTradingDesktopApp.Api.Models.UserRegistrationModel model);
    }
}
