using Application.Interfaces;
using Application.Services;
using FluentAssertions;
using Moq;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Tests.Application.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly AuthTokenService _tokenService;

        public TokenServiceTests()
        {
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _tokenService = new AuthTokenService(_tokenGeneratorMock.Object);
        }

        [Fact]
        public void GenerateToken_ShouldCallTokenGeneratorWithCorrectClaims()
        {
            // Arrange
            var user = new AuthResult
            {
                UserId = Guid.NewGuid(),
                Nom = "Doe",
                Prenom = "John",
                RoleId = 2
            };

            _tokenGeneratorMock
                .Setup(t => t.CreateToken(It.IsAny<IEnumerable<Claim>>()))
                .Returns("fake-jwt-token");

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            token.Should().Be("fake-jwt-token");

            _tokenGeneratorMock.Verify(t => t.CreateToken(It.Is<IEnumerable<Claim>>(claims =>
                // Vérifie que les claims contiennent exactement les informations de l'utilisateur
                claims.Any(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.UserId.ToString()) &&
                claims.Any(c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == user.Prenom) &&
                claims.Any(c => c.Type == JwtRegisteredClaimNames.FamilyName && c.Value == user.Nom) &&
                claims.Any(c => c.Type == "RoleId" && c.Value == user.RoleId.ToString())
            )), Times.Once);
        }

        [Fact]
        public void GenerateToken_ShouldHandleNullNamesGracefully()
        {
            // Arrange
            var user = new AuthResult
            {
                UserId = Guid.NewGuid(),
                Nom = null,
                Prenom = null,
                RoleId = 1
            };

            _tokenGeneratorMock
                .Setup(t => t.CreateToken(It.IsAny<IEnumerable<Claim>>()))
                .Returns("fake-jwt-token");

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            token.Should().Be("fake-jwt-token");

            _tokenGeneratorMock.Verify(t => t.CreateToken(It.Is<IEnumerable<Claim>>(claims =>
                claims.Any(c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == "") &&
                claims.Any(c => c.Type == JwtRegisteredClaimNames.FamilyName && c.Value == "")
            )), Times.Once);
        }
    }
}
