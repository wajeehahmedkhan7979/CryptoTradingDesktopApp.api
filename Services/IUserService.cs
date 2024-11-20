using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    // IUserService.cs  
    public interface IUserService
    {
        Task<string?> LoginUserAsync(UserLoginModel model);
        Task<RegistrationResult> RegisterUserAsync(Models.UserRegistrationModel model);
    }


}
