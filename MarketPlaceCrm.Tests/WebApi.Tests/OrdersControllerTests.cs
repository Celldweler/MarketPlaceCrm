using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MarketPlaceCrm.WebApi;
using MarketPlaceCrm.WebApi.ViewModels.OrdersDtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace MarketPlaceCrm.Tests.WebApi.Tests
{
    public class OrdersControllerTests
    {
        private readonly HttpClient _client;

        public OrdersControllerTests()
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
        [InlineData("POST")]
        public async Task Checkout(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/orders");
            var order = new OrderDto
            {
                Address = "",
                Country = "UK",
                Region = "Odesska oblast",
                PostCode = "65000",
                City = "Odessa",

                CartProducts = new List<CartProductDto>()
                {
                    new CartProductDto { Qty = 1, ProductId = 1, StockId = 1 },
                }
            };
            var orderJson = JsonConvert.SerializeObject(order);

            await _client.PostAsync("/api/orders", new StringContent(orderJson));
            
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