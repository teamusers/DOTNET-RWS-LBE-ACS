using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RWS_LBE_ACS.Common;    

public class JwtInterceptorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly byte[] _secretKey;

    public JwtInterceptorMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _secretKey = Convert.FromBase64String(config["Jwt:Secret"]!);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader))
        {
            // 2) Missing token → 401 + your standardized envelope
            await WriteErrorResponse(context, ApiResponse.MissingAuthTokenErrorResponse());
            return;
        }

        var parts = authHeader.Split(' ');
        if (parts.Length != 2 ||
            !parts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
        {
            await WriteErrorResponse(context, ApiResponse.InvalidAuthTokenErrorResponse());
            return;
        }

        var tokenString = parts[1];
        // quick sanity check for well-formed JWT
        if (tokenString.Count(c => c == '.') != 2)
        {
            await WriteErrorResponse(context, ApiResponse.InvalidAuthTokenErrorResponse());
            return;
        }

        var handler = new JwtSecurityTokenHandler();
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = handler.ValidateToken(
                tokenString, validationParams, out var validatedToken);

            // ensure we got an HMAC-SHA256 token
            if (!(validatedToken is JwtSecurityToken jwt) ||
                !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                        StringComparison.OrdinalIgnoreCase))
            {
                await WriteErrorResponse(context, ApiResponse.InvalidSignatureErrorResponse());
                return;
            }

            // stash the claims & app_id
            context.User = principal;
            context.Items["app_id"] =
                principal.Claims.FirstOrDefault(c => c.Type == "app_id")?.Value;

            // proceed to controller
            await _next(context);
        }
        catch (SecurityTokenException)
        {
            // expired, bad signature, malformed, etc.
            await WriteErrorResponse(context, ApiResponse.InvalidAuthTokenErrorResponse());
            return;
        }
    }

    // 3) new helper that takes your ApiResponse<object> and writes it out
    private static Task WriteErrorResponse(HttpContext ctx, ApiResponse<object> errorResponse)
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        ctx.Response.ContentType = "application/json";
        return ctx.Response.WriteAsJsonAsync(errorResponse);
    }
}
