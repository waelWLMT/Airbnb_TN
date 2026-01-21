using Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Tests.Application.Services
{
    public class JwtTokenGeneratorTests
    {
        private readonly JwtTokenGenerator _generator;

        public JwtTokenGeneratorTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "Ekcsu4hOdsaWbgy86TnNVRvHGDv3MZUj9HQYDnHAq2PtgYUTCRhjyEm3HCVvhZ2v7B0F2bg9BsIh8KlstOpQ+Q=="},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _generator = new JwtTokenGenerator(configuration);
        }

        [Fact]
        public void CreateToken_ShouldReturnNonEmptyToken()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "test@test.com")
            };

            // Act
            var token = _generator.CreateToken(claims);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void CreateToken_ShouldIncludeAllClaims()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var email = "user@test.com";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
            };

            // Act
            var token = _generator.CreateToken(claims);

            // Decode token
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Assert
            jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);
            jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Email && c.Value == email);
        }

        [Fact]
        public void CreateToken_ShouldHaveCorrectIssuerAndAudience()
        {
            // Arrange
            var claims = new List<Claim>();

            // Act
            var token = _generator.CreateToken(claims);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Assert
            jwt.Issuer.Should().Be("TestIssuer");
            jwt.Audiences.Should().ContainSingle(a => a == "TestAudience");
        }

        [Fact]
        public void CreateToken_ShouldHaveExpirationApproximatelyOneHourFromNow()
        {
            // Arrange
            var claims = new List<Claim>();

            // Act
            var token = _generator.CreateToken(claims);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Assert
            var expected = DateTime.UtcNow.AddHours(1);
            jwt.ValidTo.Should().BeCloseTo(expected, TimeSpan.FromMinutes(1));
        }
    }
}
