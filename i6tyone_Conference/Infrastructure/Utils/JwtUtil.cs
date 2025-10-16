using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public class JwtUtil
    {
        private readonly JwtSettings _jwtSettings;

        public JwtUtil(IOptions<JwtSettings> jwtSettings) => _jwtSettings = jwtSettings.Value;

        public string GenerateAccessToken(JwtPayloadModel payload)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, payload.id),
                new Claim(JwtRegisteredClaimNames.Name, payload.name),
                new Claim("phoneNumber", payload.phoneNumber),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddSeconds(_jwtSettings.AccessTokenExpireSeconds),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(JwtPayloadModel payload)
        {
            // 1. Header
            var header = new { alg = "HS256", typ = "JWT" };
            var headerJson = JsonConvert.SerializeObject(header);
            var headerBase64 = Base64UrlEncoder.Encode(headerJson);

            // 2. Payload
            payload.iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            payload.exp = payload.iat + _jwtSettings.RefreshTokenExpireSeconds;

            var payloadJson = JsonConvert.SerializeObject(payload);
            var payloadEncrypted = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.JWT_PAYLOAD)?.AESEncrypt(payloadJson);
            var payloadBase64 = Base64UrlEncoder.Encode(payloadEncrypted);

            // 3. Signature
            var signatureRaw = $"{headerBase64}.{payloadBase64}";
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            using var hmac = new HMACSHA256(key);
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureRaw));
            var signatureBase64 = Base64UrlEncoder.Encode(signatureBytes);

            return $"{headerBase64}.{payloadBase64}.{signatureBase64}";
        }
    }
}