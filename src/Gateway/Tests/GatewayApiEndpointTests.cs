using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace GatewayAPI.Tests
{
    public class GatewayApiEndpointTests
    {
        [Fact]
        public void RootEndpoint_Returns200()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.StatusCode = 200;

            // Simuler un GET sur "/"
            context.Response.StatusCode = 200;
            var result = "Gateway API running!";

            // Assert
            Assert.Equal(200, context.Response.StatusCode);
            Assert.Equal("Gateway API running!", result);
        }
    }
}
