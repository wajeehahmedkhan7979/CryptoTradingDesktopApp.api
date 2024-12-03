// CryptoTradingDesktopApp.Api.Models/UserRegistrationModel.cs
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class UserRegistrationModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; internal set; } 
        public string LastName { get; internal set; }  
    }
}