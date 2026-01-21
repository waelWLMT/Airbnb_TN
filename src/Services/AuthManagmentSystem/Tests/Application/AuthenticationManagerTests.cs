using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using FluentAssertions;
using Moq;
using Shared.Dtos;
using Shared.Enums;
using Xunit;

namespace Tests.Application.Services
{
    public class AuthenticationManagerTests
    {
        private readonly Mock<IAuthenticatorService> _jwtProviderMock;
        private readonly Mock<IAuthenticatorService> _googleProviderMock;
        private readonly AuthenticationManager _orchestrator;

        public AuthenticationManagerTests()
        {
            _jwtProviderMock = new Mock<IAuthenticatorService>();
            _jwtProviderMock.Setup(p => p.ServiceName).Returns(AuthServiceName.Jwt);

            _googleProviderMock = new Mock<IAuthenticatorService>();
            _googleProviderMock.Setup(p => p.ServiceName).Returns(AuthServiceName.Google);

            var providers = new List<IAuthenticatorService>
            {
                _jwtProviderMock.Object,
                _googleProviderMock.Object
            };

            _orchestrator = new AuthenticationManager(providers);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldCallCorrectProvider_WhenServiceExists()
        {
            // Arrange
            var request = new AuthRequest { ServiceName = AuthServiceName.Jwt };
            var expectedResult = new AuthResult { Email = "test@test.com", IsActive = true };

            _jwtProviderMock
                .Setup(p => p.AuthenticateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _orchestrator.AuthenticateAsync(request);

            // Assert
            result.Should().Be(expectedResult);
            _jwtProviderMock.Verify(p => p.AuthenticateAsync(request), Times.Once);
            _googleProviderMock.Verify(p => p.AuthenticateAsync(It.IsAny<AuthRequest>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrowNotSupportedException_WhenProviderDoesNotExist()
        {
            // Arrange
            var request = new AuthRequest { ServiceName = (AuthServiceName)999 }; // valeur invalide

            // Act
            Func<Task> act = () => _orchestrator.AuthenticateAsync(request);

            // Assert
            await act.Should().ThrowAsync<NotSupportedException>()
                     .WithMessage("Auth provider not supported");

            _jwtProviderMock.Verify(p => p.AuthenticateAsync(It.IsAny<AuthRequest>()), Times.Never);
            _googleProviderMock.Verify(p => p.AuthenticateAsync(It.IsAny<AuthRequest>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldCallOnlyTheSelectedProvider()
        {
            // Arrange
            var request = new AuthRequest { ServiceName = AuthServiceName.Google };
            var expectedResult = new AuthResult { Email = "google@test.com", IsActive = true };

            _googleProviderMock
                .Setup(p => p.AuthenticateAsync(request))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _orchestrator.AuthenticateAsync(request);

            // Assert
            result.Should().Be(expectedResult);
            _googleProviderMock.Verify(p => p.AuthenticateAsync(request), Times.Once);
            _jwtProviderMock.Verify(p => p.AuthenticateAsync(It.IsAny<AuthRequest>()), Times.Never);
        }
    }
}
