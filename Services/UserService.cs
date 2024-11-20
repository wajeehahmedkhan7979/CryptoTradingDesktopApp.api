using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;


namespace CryptoTradingDesktopApp.Api.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("path/to/your/serviceAccountKey.json")
                });
            }
        }

        public Task<string?> LoginUserAsync(Models.UserLoginModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResult> RegisterUserAsync(UserRegistrationModel model)
        {
            try
            {
                var userRecordArgs = new UserRecordArgs()
                {
                    Email = model.Email,
                    Password = model.Password,
                    DisplayName = model.FullName
                };

                var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);
                return new AuthResult { IsSuccess = true, UserId = userRecord.Uid };
            }
            catch (FirebaseAuthException ex)
            {
                return new AuthResult { IsSuccess = false, Errors = new[] { ex.Message } };
            }
        }

        public Task RegisterUserAsync(Models.UserRegistrationModel model)
        {
            throw new NotImplementedException();
        }

        async Task<string?> IUserService.LoginUserAsync(UserLoginModel model)
        {
            var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(model.Email);
            return await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(user.Uid);
        }
    }

    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string? UserId { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();  // Initialized to an empty array
        public bool Success { get; internal set; }
        public string? Message { get; internal set; }
        public DateTime ExpiresAt { get; internal set; }
        public string? Token { get; internal set; }
    }

    public class UserRegistrationModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? FirstName { get; internal set; }
        public string? LastName { get; internal set; }
    }

    public class UserLoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
