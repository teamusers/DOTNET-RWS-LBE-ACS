using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RWS_LBE_ACS.Services
{
    public static class TokenInterceptor
    {
        // default secret, _must_ be replaced at startup with a ≥32-byte key
        private static byte[] _jwtSecret = Encoding.UTF8.GetBytes("lbe-jwt-secretKey");

        /// <summary>
        /// Override the JWT signing secret by passing in the raw key bytes (≥32 bytes for HS256).
        /// </summary>
        public static void SetJwtSecret(byte[] secretBytes)
        {
            if (secretBytes == null || secretBytes.Length < 32)
                throw new ArgumentException(
                    "JWT secret must be at least 256 bits (32 bytes) in length.",
                    nameof(secretBytes));

            _jwtSecret = secretBytes;
        }

        /// <summary>
        /// Convenience overload: accepts a Base64-encoded secret string.
        /// </summary>
        public static void SetJwtSecret(string base64Secret)
        {
            if (string.IsNullOrWhiteSpace(base64Secret))
                throw new ArgumentException("JWT secret string cannot be null or empty.", nameof(base64Secret));

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(base64Secret);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("JWT secret must be valid Base64.", nameof(base64Secret), ex);
            }

            SetJwtSecret(bytes);
        }

        /// <summary>
        /// Generates a JWT for the given AppId, valid for 4 hours.
        /// </summary>
        public static string GenerateToken(string appId)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddHours(4);

            var claims = new[]
            {
                new Claim("app_id", appId),
                new Claim(JwtRegisteredClaimNames.Iat,
                          new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),
                          ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Exp,
                          new DateTimeOffset(expires).ToUnixTimeSeconds().ToString(),
                          ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss, "lbe-api")
            };

            var key = new SymmetricSecurityKey(_jwtSecret);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
