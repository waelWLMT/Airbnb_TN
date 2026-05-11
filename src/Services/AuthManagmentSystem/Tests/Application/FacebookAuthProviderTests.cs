using Shared.Enums;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;
using Application.AuthProviders;

namespace UnitTests.Application
{
    public class FacebookAuthProviderTests
    {
        private readonly FacebookAuthProvider _provider;

        public FacebookAuthProviderTests()
        {
            _provider = new FacebookAuthProvider();
        }

        [Fact]
        public void ServiceName_ShouldReturnFacebook()
        {
            _provider.ServiceName.Should().Be(AuthServiceName.Facebook);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrowNotImplementedException()
        {
            var request = new Shared.Dtos.AuthRequest
            {
                ServiceName = AuthServiceName.Facebook,
                ExternalToken = "dummy-token"
            };

            await Assert.ThrowsAsync<NotImplementedException>(async () =>
            {
                await _provider.AuthenticateAsync(request);
            });
        }
    }
}
