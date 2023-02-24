using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MarketPlaceCrm.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace MarketPlaceCrm.Tests.WebApi.Tests
{
    public class TestControllerTests
    {
        private readonly HttpClient _client;

        public TestControllerTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());

            _client = server.CreateClient();
        }
        [Fact]
        public void Test1()
        {
        }

        [Theory]
        [InlineData("GET")]
        public async Task GetMessageFromTestApiController(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/test");
            
            // Act
            var response = await _client.SendAsync(request);
            
            // Assert
            response.EnsureSuccessStatusCode();
            var message = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(message, "testcontroller is work!");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}