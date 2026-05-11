using Shared.Dtos;
using Shared.Enums;
using Application.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Application.AuthProviders;

namespace UnitTests.Application
{
    public class JwtAuthProviderTests
    {
        private readonly Mock<IUserManagementHttpClient> _userClientMock;
        private readonly JwtAuthProvider _jwtAuthService;

        public JwtAuthProviderTests()
        {
            _userClientMock = new Mock<IUserManagementHttpClient>();
            _jwtAuthService = new JwtAuthProvider(_userClientMock.Object);
        }

        [Fact]
        public void ServiceName_ShouldReturnJwt()
        {
            // Arrange & Act
            var serviceName = _jwtAuthService.ServiceName;

            // Assert
            serviceName.Should().Be(AuthServiceName.Jwt);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnAuthResult_WhenValidCredentials()
        {
            // Arrange
            var request = new AuthRequest
            {
                ServiceName = AuthServiceName.Jwt,
                Email = "user@test.com",
                Password = "password123"
            };

            var expectedUser = new AuthResult
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                Nom = "Doe",
                Prenom = "John",
                RoleId = 1,
                IsActive = true
            };

            _userClientMock
                .Setup(x => x.ValidateCredentials(request.Email, request.Password, default))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _jwtAuthService.AuthenticateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenInvalidCredentials()
        {
            // Arrange
            var request = new AuthRequest
            {
                ServiceName = AuthServiceName.Jwt,
                Email = "user@test.com",
                Password = "wrongpassword"
            };

            _userClientMock
                .Setup(x => x.ValidateCredentials(request.Email, request.Password, default))
                .ReturnsAsync((AuthResult?)null);

            // Act
            var result = await _jwtAuthService.AuthenticateAsync(request);

            // Assert
            result.Should().BeNull();
        }
    }
}
