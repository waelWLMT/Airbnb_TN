using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Shared.Dtos;

namespace Tests.Presentation
{
    
        public class AuthenticationsControllerTests
        {
            private readonly Mock<IAuthenticationManager> _authOrchestratorMock;
            private readonly Mock<IAuthTokenService> _tokenServiceMock;
            private readonly AuthenticationsController _controller;
           
            public AuthenticationsControllerTests()
            {
                _authOrchestratorMock = new Mock<IAuthenticationManager>();
                _tokenServiceMock = new Mock<IAuthTokenService>();

                _controller = new AuthenticationsController(
                    _authOrchestratorMock.Object,
                    _tokenServiceMock.Object);
            }

            [Fact]
            public async Task Login_ShouldReturnOk_WhenAuthResultIsActive()
            {
                // Arrange
                var request = new AuthRequest
                {
                    Email = "test@test.com",
                    Password = "password"
                };

                var authResult = new AuthResult
                {
                    UserId = Guid.NewGuid(),
                    Email = "test@test.com",
                    IsActive = true
                };

                _authOrchestratorMock
                    .Setup(x => x.AuthenticateAsync(request))
                    .ReturnsAsync(authResult);

                _tokenServiceMock
                    .Setup(x => x.GenerateToken(authResult))
                    .Returns("jwt-token");

                // Act
                var result = await _controller.Login(request);

                // Assert
                result.Should().BeOfType<OkObjectResult>();

                var okResult = result as OkObjectResult;
                okResult!.Value.Should().BeOfType<LoginResponseDto>();

                var response = okResult.Value as LoginResponseDto;
                response!.User.Should().Be(authResult);
                response.Token.Should().Be("jwt-token");
                response.Expiration.Should().BeAfter(DateTime.UtcNow);

                _authOrchestratorMock.Verify(
                    x => x.AuthenticateAsync(request),
                    Times.Once);

                _tokenServiceMock.Verify(
                    x => x.GenerateToken(authResult),
                    Times.Once);
            }

            [Fact]
            public async Task Login_ShouldReturnUnauthorized_WhenAuthResultIsNull()
            {
                // Arrange
                var request = new AuthRequest();

                _authOrchestratorMock
                    .Setup(x => x.AuthenticateAsync(request))
                    .ReturnsAsync((AuthResult?)null);

                // Act
                var result = await _controller.Login(request);

                // Assert
                result.Should().BeOfType<UnauthorizedResult>();

                _tokenServiceMock.Verify(
                    x => x.GenerateToken(It.IsAny<AuthResult>()),
                    Times.Never);
            }

            [Fact]
            public async Task Login_ShouldReturnUnauthorized_WhenUserIsInactive()
            {
                // Arrange
                var request = new AuthRequest();

                var authResult = new AuthResult
                {
                    IsActive = false
                };

                _authOrchestratorMock
                    .Setup(x => x.AuthenticateAsync(request))
                    .ReturnsAsync(authResult);

                // Act
                var result = await _controller.Login(request);

                // Assert
                result.Should().BeOfType<UnauthorizedResult>();

                _tokenServiceMock.Verify(
                    x => x.GenerateToken(It.IsAny<AuthResult>()),
                    Times.Never);
            }
        }

  
}
