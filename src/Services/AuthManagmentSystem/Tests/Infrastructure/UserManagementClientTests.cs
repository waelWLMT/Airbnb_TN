using Infrastructure.Clients;
using Shared.Dtos;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Infrastructure.Clients
{
    public class UserManagementClientTests
    {
        [Fact]
        public async Task ValidateCredentials_ShouldReturnNull_WhenEmailOrPasswordIsNull()
        {
            // Arrange
            var client = new UserManagementHttpClient(new HttpClient());

            // Act
            var result1 = await client.ValidateCredentials(null, "password", CancellationToken.None);
            var result2 = await client.ValidateCredentials("email@test.com", null, CancellationToken.None);
            var result3 = await client.ValidateCredentials(null, null, CancellationToken.None);

            // Assert
            result1.Should().BeNull();
            result2.Should().BeNull();
            result3.Should().BeNull();
        }

        [Fact]
        public async Task ValidateCredentials_ShouldReturnNull_WhenUnauthorized()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.Unauthorized
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var client = new UserManagementHttpClient(httpClient);

            // Act
            var result = await client.ValidateCredentials("test@test.com", "password", CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ValidateCredentials_ShouldReturnAuthResult_WhenSuccessful()
        {
            // Arrange
            var expectedUser = new AuthResult
            {
                UserId = Guid.NewGuid(),
                Email = "user@test.com",
                Nom = "Doe",
                Prenom = "John",
                RoleId = 1
            };

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri!.AbsolutePath == "/api/users/ValidateCredentials"
                   ),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = JsonContent.Create(expectedUser)
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var client = new UserManagementHttpClient(httpClient);

            // Act
            var result = await client.ValidateCredentials("user@test.com", "password", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(expectedUser.UserId);
            result.Email.Should().Be(expectedUser.Email);
            result.Nom.Should().Be(expectedUser.Nom);
            result.Prenom.Should().Be(expectedUser.Prenom);
            result.RoleId.Should().Be(expectedUser.RoleId);
        }

        [Fact]
        public async Task ValidateCredentials_ShouldThrowException_WhenNonSuccessStatus()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.BadRequest
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var client = new UserManagementHttpClient(httpClient);

            // Act
            Func<Task> act = async () =>
            {
                await client.ValidateCredentials("user@test.com", "password", CancellationToken.None);
            };

            // Assert
            await act.Should().ThrowAsync<HttpRequestException>();
        }
    }
}
