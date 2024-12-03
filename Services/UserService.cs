using CryptoTradingDesktopApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class UserService : IUserService
    {
        private readonly CryptoDbContext _context;

        public UserService(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<RegistrationResult> RegisterUserAsync(UserRegistrationModel model)
        {
            // Check if a user with the same email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return new RegistrationResult
                {
                    IsSuccess = false, // Registration failed
                    Message = "Email is already registered.",
                    Errors = new List<string> { "Email is already registered." }
                };
            }

            // Hash the password using BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Create a new user
            var newUser = new UserModel
            {
                UserId = Guid.NewGuid(),
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = passwordHash,
                DateRegistered = DateTime.UtcNow,
                IsVerified = false // Set this based on your application's requirements
            };

            // Add user to the database
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            // Return a successful registration result
            return new RegistrationResult
            {
                IsSuccess = true, // Registration succeeded
                Message = "User registered successfully.",
                Errors = new List<string>() // No errors
            };
        }

        public async Task<string?> LoginUserAsync(UserLoginModel model)
        {
            // Retrieve the user from the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return null; // User not found
            }

            // Verify the password
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return null; // Password does not match
            }

            // Generate a JWT token (stubbed here, implement your token generation logic)
            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(UserModel user)
        {
            // Replace with your actual JWT token generation logic
            return "sample_jwt_token";
        }
    }
}
