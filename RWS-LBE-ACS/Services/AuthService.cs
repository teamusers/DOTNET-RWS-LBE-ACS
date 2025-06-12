using System;
using System.Security.Cryptography;
using System.Text;
using RWS_LBE_ACS.DTOs.Requests;

namespace RWS_LBE_ACS.Services.Authentication
{
    public interface IAuthService
    {
        AuthRequest GenerateSignature(string appId, string secretKey);
        AuthRequest GenerateSignatureWithParams(
            string appId,
            string nonce,
            string timestamp,
            string secretKey);
    }

    public class AuthService : IAuthService
    {
        private static readonly string _chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random _rng = new();

        public AuthRequest GenerateSignature(string appId, string secretKey)
        {
            var nonce = RandomNonce(16);
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            return ComputeSignature(appId, nonce, timestamp, secretKey);
        }

        public AuthRequest GenerateSignatureWithParams(
            string appId,
            string nonce,
            string timestamp,
            string secretKey)
        {
            return ComputeSignature(appId, nonce, timestamp, secretKey);
        }

        private AuthRequest ComputeSignature(
            string appId,
            string nonce,
            string timestamp,
            string secretKey)
        {
            var baseString = $"{appId}{timestamp}{nonce}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(baseString));
            var signature = BitConverter
                               .ToString(hash)
                               .Replace("-", "")
                               .ToLowerInvariant();

            return new AuthRequest
            {
                Nonce = nonce,
                Timestamp = timestamp,
                Signature = signature
            };
        }

        private string RandomNonce(int length)
        {
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(_chars[_rng.Next(_chars.Length)]);
            return sb.ToString();
        }
    }
}
