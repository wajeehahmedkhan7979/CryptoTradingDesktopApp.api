using System;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly CryptoDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(CryptoDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        }

        private string GenerateJwtToken(UserModel user)
        {
            var key = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
