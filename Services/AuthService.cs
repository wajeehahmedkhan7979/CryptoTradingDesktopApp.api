using System;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly CryptoDbContext _context;

        public AuthService(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResult> LoginAsync(UserLoginModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var token = GenerateJwtToken(user);
            return new AuthResult
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            throw new NotImplementedException();
        }

        public Task<AuthResult> LoginAsync(Models.UserLoginModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResult> RegisterAsync(UserRegistrationModel model)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Email already registered."
                };
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var user = new UserModel
            {
                UserId = Guid.NewGuid(),
                Email = model.Email,
                FirstName = model.FirstName ?? string.Empty,
                LastName = model.LastName ?? string.Empty,
                PasswordHash = passwordHash,
                DateRegistered = DateTime.UtcNow,
                IsVerified = false
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Success = true,
                Message = "Registration successful"
            };

            throw new NotImplementedException();
        }

        public Task<AuthResult> RegisterAsync(Models.UserRegistrationModel model)
        {
            throw new NotImplementedException();
        }

        private static string GenerateJwtToken(UserModel user)
        {
            // Implement your JWT token generation logic here
            // For now, returning a placeholder
            return "jwt_token";
        }
    }
}