using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GatewayApi.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace GatewayAPI.Tests.Middlewares
{
    public class RequestLoggingMiddlewareTests
    {
        private string GenerateToken(bool expired = false)
        {
            // Clé 256 bits pour HmacSha256
            var keyBytes = Encoding.UTF8.GetBytes("01234567890123456789012345678901");
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim("RoleId", "1")
            };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: "TestIssuer",
                audience: "TestAudience",
                claims: claims,
                expires: expired ? DateTime.UtcNow.AddMinutes(-5) : DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        [Fact]
        public async Task InvokeAsync_CallsNext_AndLogsInformation()
        {
            var mockLogger = new Mock<ILogger<RequestLoggingMiddleware>>();

            bool nextCalled = false;
            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                ctx.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            var middleware = new RequestLoggingMiddleware(next, mockLogger.Object);

            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/test";
            context.Request.Headers["Authorization"] = $"Bearer {GenerateToken()}";

            await middleware.InvokeAsync(context);

            Assert.True(nextCalled);
            Assert.Equal(200, context.Response.StatusCode);

            // Vérification du log info
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task InvokeAsync_ExpiredToken_LogsWarning()
        {
            var mockLogger = new Mock<ILogger<RequestLoggingMiddleware>>();
            bool nextCalled = false;

            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                ctx.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            var middleware = new RequestLoggingMiddleware(next, mockLogger.Object);

            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/protected";
            context.Request.Headers["Authorization"] = $"Bearer {GenerateToken(expired: true)}";

            await middleware.InvokeAsync(context);

            Assert.True(nextCalled);
            Assert.Equal(200, context.Response.StatusCode);

            // Vérification du log warning
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.AtLeastOnce
            );
        }
    }
}
