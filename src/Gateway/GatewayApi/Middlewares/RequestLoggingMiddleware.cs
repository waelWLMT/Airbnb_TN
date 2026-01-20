using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GatewayApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var path = context.Request.Path;
            var method = context.Request.Method;

            string user = "Anonymous";
            string tokenIssuer = "None";
            string tokenType = "None";
            bool tokenExpired = false;

            // Vérifie si un JWT est présent
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var tokenStr = authHeader.Substring("Bearer ".Length).Trim();
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(tokenStr))
                    {
                        var token = handler.ReadJwtToken(tokenStr);
                        tokenIssuer = token.Issuer ?? "Unknown";

                        // Détecte type de token
                        tokenType = tokenIssuer switch
                        {
                            "AuthManagementSystem" => "Internal JWT",
                            "accounts.google.com" => "Google JWT",
                            "facebook.com" => "Facebook JWT",
                            _ => "Unknown"
                        };

                        // Vérifie expiration
                        var exp = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                        if (exp != null && long.TryParse(exp, out long expSeconds))
                        {
                            var expDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
                            if (expDate < DateTimeOffset.UtcNow)
                                tokenExpired = true;
                        }

                        // Utilisateur (claim "sub" ou "name")
                        user = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value
                               ?? token.Claims.FirstOrDefault(c => c.Type == "name")?.Value
                               ?? "Unknown";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Invalid token detected: {Message}", ex.Message);
                }
            }

            // Détermine si la route est publique (metadata AllowAnonymous)
            bool isPublicRoute = context.GetEndpoint()?.Metadata
                .GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null;

            // Log avant traitement
            _logger.LogInformation(
                "[Request] {Method} {Path} | User: {User} | TokenType: {TokenType} | Issuer: {Issuer} | Expired: {Expired} | PublicRoute: {IsPublic}",
                method, path, user, tokenType, tokenIssuer, tokenExpired, isPublicRoute
            );

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                var status = context.Response.StatusCode;
                _logger.LogInformation(
                    "[Response] {Method} {Path} | Status: {Status} | Duration: {Duration} ms",
                    method, path, status, sw.ElapsedMilliseconds
                );

                // Alerte si token expiré sur route protégée
                if (!isPublicRoute && tokenExpired)
                {
                    _logger.LogWarning(
                        "Expired token used on protected route: {Method} {Path} | User: {User} | TokenType: {TokenType}",
                        method, path, user, tokenType
                    );
                }
            }
        }
    }
}
