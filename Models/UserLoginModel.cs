// CryptoTradingDesktopApp.Api.Models/UserLoginModel.cs
using System.ComponentModel.DataAnnotations;


namespace CryptoTradingDesktopApp.Api.Models
{
    public class UserLoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
