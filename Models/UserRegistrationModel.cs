// CryptoTradingDesktopApp.Api.Models/UserRegistrationModel.cs
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class UserRegistrationModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? FirstName { get; internal set; } // Nullable as they are optional
        public string? LastName { get; internal set; }  // Nullable as they are optional
    }
}