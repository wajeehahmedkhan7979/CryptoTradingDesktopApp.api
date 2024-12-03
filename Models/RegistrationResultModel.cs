namespace CryptoTradingDesktopApp.Api.Models
{
    public class RegistrationResult
    {
        public bool IsSuccess { get; set; } // Changed from IsSuccessful to IsSuccess
        public string Message { get; set; } // Changed from Errors to Message
        public List<string> Errors { get; set; } = new List<string>(); // Add this to store error messages if any
    }
}
