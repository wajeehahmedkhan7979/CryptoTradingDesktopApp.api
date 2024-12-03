// CryptoTradingDesktopApp.Api.Models/AuthResult.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string? UserId { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();
        public bool Success { get; internal set; }
        public string? Message { get; internal set; } = string.Empty; // Allowing nullable
        public DateTime ExpiresAt { get; internal set; }
        public string? Token { get; internal set; } = string.Empty;
    }

}