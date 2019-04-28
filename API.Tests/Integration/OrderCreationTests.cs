using System;
using System.Net.Http;
using System.Threading.Tasks;
using API.DTO;
using API.Tests.Util;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Xunit;

namespace API.Tests.Integration
{
    [Collection(nameof(TestCollection))]
    public class OrderCreationTests
    {
        public OrderCreationTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly TestFixture _fixture;

        [Fact]
        public async Task Normal_Orders_Must_Be_In_Normal_Orders_Container()
        {
            var httpResponse = await _fixture.Client.PostAsJsonAsync("api/createorder", new CreateOrderRequest
            {
                CustomerId = 1,
                ProductId = 1,
                OrderDate = DateTime.UtcNow,
                Quantity = 10,
                Price = 100
            });

            httpResponse.EnsureSuccessStatusCode();

            await Task.Delay(5000);

            var createOrderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(await httpResponse.Content.ReadAsStringAsync());

            Assert.NotNull(createOrderResponse);

            var order = _fixture.NormalOrdersContainer.GetBlockBlobReference($"{createOrderResponse.OrderId}.json");
            var exists = await order.ExistsAsync();


            Assert.True(exists);

            var vipOrderContent = await order.DownloadTextAsync();

            Assert.NotNull(vipOrderContent);
        }

        [Fact]
        public async Task Vip_Orders_Must_Be_In_Vip_Orders_Container()
        {
            var httpResponse = await _fixture.Client.PostAsJsonAsync("api/createorder", new CreateOrderRequest
            {
                CustomerId = 2,
                ProductId = 1,
                OrderDate = DateTime.UtcNow,
                Quantity = 10,
                Price = 100
            });

            httpResponse.EnsureSuccessStatusCode();

            await Task.Delay(2500);

            var createOrderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(await httpResponse.Content.ReadAsStringAsync());

            Assert.NotNull(createOrderResponse);

            var order = _fixture.VipOrdersContainer.GetBlockBlobReference($"{createOrderResponse.OrderId}.json");
            var exists = await order.ExistsAsync();


            Assert.True(exists);

            var vipOrderContent = await order.DownloadTextAsync();

            Assert.NotNull(vipOrderContent);
        }
    }
}