using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GatewayApi.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace GatewayAPI.Tests.Middlewares
{
    public class JwtMiddlewareTests
    {
        private JwtMiddleware CreateMiddleware(IConfiguration config, RequestDelegate next)
            => new(next, config);

        private IConfiguration CreateConfiguration()
        {
            // Clé Base64 256 bits pour HmacSha256
            var secretBytes = Encoding.UTF8.GetBytes("01234567890123456789012345678901"); // 32 bytes = 256 bits
            var base64Secret = Convert.ToBase64String(secretBytes);

            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", base64Secret},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private string GenerateToken(IConfiguration config, bool expired = false)
        {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim("RoleId", "1")
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: expired ? DateTime.UtcNow.AddMinutes(-10) : DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        [Fact]
        public async Task InvokeAsync_ValidToken_SetsHttpContextUser()
        {
            // Arrange
            var config = CreateConfiguration();
            var token = GenerateToken(config);

            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = new StringValues($"Bearer {token}");

            var middleware = CreateMiddleware(config, (ctx) =>
            {
                Assert.True(ctx.User.Identity!.IsAuthenticated);
                Assert.Equal("TestUser", ctx.User.Identity.Name);
                return Task.CompletedTask;
            });

            // Act
            await middleware.InvokeAsync(context);
        }

        [Fact]
        public async Task InvokeAsync_MissingToken_Returns401()
        {
            // Arrange
            var config = CreateConfiguration();
            var context = new DefaultHttpContext();

            var middleware = CreateMiddleware(config, (ctx) =>
            {
                Assert.False(true); // ne doit pas être appelé
                return Task.CompletedTask;
            });

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ExpiredToken_Returns401()
        {
            // Arrange
            var config = CreateConfiguration();
            var token = GenerateToken(config, expired: true);

            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = new StringValues($"Bearer {token}");

            var middleware = CreateMiddleware(config, (ctx) =>
            {
                Assert.False(true); // ne doit pas être appelé
                return Task.CompletedTask;
            });

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }
    }
}
