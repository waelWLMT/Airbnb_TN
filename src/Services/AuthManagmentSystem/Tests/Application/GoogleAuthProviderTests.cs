using Shared.Enums;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;
using Application.AuthProviders;

namespace UnitTests.Application
{
    public class GoogleAuthProviderTests
    {
        private readonly GoogleAuthProvider _provider;

        public GoogleAuthProviderTests()
        {
            _provider = new GoogleAuthProvider();
        }

        [Fact]
        public void ServiceName_ShouldReturnGoogle()
        {
            _provider.ServiceName.Should().Be(AuthServiceName.Google);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrowNotImplementedException()
        {
            var request = new Shared.Dtos.AuthRequest
            {
                ServiceName = AuthServiceName.Google,
                ExternalToken = "dummy-token"
            };

            await Assert.ThrowsAsync<NotImplementedException>(async () =>
            {
                await _provider.AuthenticateAsync(request);
            });
        }
    }
}
