using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Model;

namespace GatewayApi.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (IsPublicRoute(context))
            {
                await _next(context);
                return;
            }

            // Vérifier header Authorization
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: No token provided");
                return;
            }

            var token = authHeader.ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Malformed token");
                return;
            }

            try
            {
                var jwtSection = _configuration.GetSection("Jwt");
                var key = Convert.FromBase64String(jwtSection["Key"]!);
                var issuer = jwtSection["Issuer"];
                var audience = jwtSection["Audience"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParams, out var validatedToken);

                // Mapping claims
                var identity = new ClaimsIdentity(principal.Claims, "jwt");
                var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == "RoleId");
                if (roleClaim != null)
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));

                context.User = new ClaimsPrincipal(identity);

                await _next(context);
            }
            catch (SecurityTokenException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token");
            }
        }   
        
        private bool IsPublicRoute(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint is null)
                return false;

            var routeModel = endpoint.Metadata.GetMetadata<RouteModel>();
            if (routeModel?.Config?.Metadata is not { } metadata)
                return false;

            return metadata.TryGetValue("AllowAnonymous", out var value)
                   && value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
