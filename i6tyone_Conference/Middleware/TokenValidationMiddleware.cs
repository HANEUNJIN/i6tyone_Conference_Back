using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eGhis_WebService_Core.Middleware
{
    public sealed class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwt;

        public TokenValidationMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtOptions)
        {
            _next = next;
            _jwt = jwtOptions.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            if (!string.IsNullOrEmpty(path) &&
                (path.StartsWith("/swagger") || path.StartsWith("/redoc") || path.StartsWith("/stoplight")))
            {
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousTokenAttribute>() != null)
            {
                await _next(context);
                return;
            }

            var bearer = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(bearer) || !bearer.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                await Respond(context, 401, "Authorization header missing or invalid");
                return;
            }

            var token = bearer.Substring("Bearer ".Length).Trim();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = !string.IsNullOrWhiteSpace(_jwt.Issuer),
                ValidIssuer = _jwt.Issuer,
                ValidateAudience = !string.IsNullOrWhiteSpace(_jwt.Audience),
                ValidAudience = _jwt.Audience,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, parameters, out var validated);

                if (validated is not JwtSecurityToken jwtTok ||
                    !string.Equals(jwtTok.Header.Alg, SecurityAlgorithms.HmacSha256, StringComparison.Ordinal))
                {
                    await Respond(context, 401, "Access token is invalid (alg mismatch)");
                    return;
                }

                var payload = MapToPayload(principal, jwtTok);

                var nowSec = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (payload.exp <= nowSec)
                {
                    await Respond(context, 401, "Access token has expired");
                    return;
                }

                context.User = principal;
                context.Items["jwt"] = payload;

                await _next(context);
            }
            catch (SecurityTokenExpiredException)
            {
                await Respond(context, 401, "Access token has expired");
            }
            catch (SecurityTokenException)
            {
                await Respond(context, 401, "Access token is invalid");
            }
            catch (Exception ex)
            {
                await Respond(context, 500, $"Token validation failed: {ex.Message}");
            }
        }

        private static JwtPayloadModel MapToPayload(ClaimsPrincipal principal, JwtSecurityToken jwtTok)
        {
            // JwtUtil 발급 시 클레임 키를 속성명 그대로 썼다고 가정
            string? s(string type) => principal.FindFirst(type)?.Value;

            var model = new JwtPayloadModel
            {
                id = s(nameof(JwtPayloadModel.id)) ?? string.Empty,
                name = s(nameof(JwtPayloadModel.name)) ?? string.Empty,
                phoneNumber = s(nameof(JwtPayloadModel.phoneNumber)) ?? string.Empty,
                iat = TryParseLong(s(nameof(JwtPayloadModel.iat))) ?? new DateTimeOffset(jwtTok.ValidFrom).ToUnixTimeSeconds(),
                exp = TryParseLong(s(nameof(JwtPayloadModel.exp))) ?? new DateTimeOffset(jwtTok.ValidTo).ToUnixTimeSeconds()
            };

            return model;
        }

        private static long? TryParseLong(string? v) => long.TryParse(v, out var x) ? x : null;

        private static async Task Respond(HttpContext ctx, int statusCode, string message)
        {
            ctx.Response.StatusCode = statusCode;
            ctx.Response.ContentType = "application/json";
            var body = new { ResultCd = statusCode.ToString(), ResultMsg = message };
            await ctx.Response.WriteAsync(JsonConvert.SerializeObject(body));
        }
    }
}
