using System.Threading.Tasks;
using API.Events;
using API.Util;
using AzureFunctions.Autofac;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace API.Functions
{
    [DependencyInjectionConfig(typeof(Bootstrapper))]
    public static class ProcessOrderFunction
    {
        [FunctionName("ProcessOrder")]
        public static async Task Run(
            [QueueTrigger("orders")] OrderReceivedEvent orderData,
            [Inject] IOrderProcessor orderProcessor,
            [Blob("viporders")] CloudBlobContainer vipOrdersContainer,
            [Blob("normalorders")] CloudBlobContainer normalOrdersContainer,
            ILogger log)
        {
            log.LogInformation($"Received order created event: {orderData.OrderTrackingId}");

            await vipOrdersContainer.CreateIfNotExistsAsync();
            await normalOrdersContainer.CreateIfNotExistsAsync();

            var orderNature = orderProcessor.GetOrderNature(orderData);

            var serializedOrderData = JsonConvert.SerializeObject(orderData);

            if (orderNature == OrderType.Vip)
            {
                var vipBlob = vipOrdersContainer.GetBlockBlobReference($"{orderData.OrderTrackingId}.json");
                await vipBlob.UploadTextAsync(JsonConvert.SerializeObject(serializedOrderData));
            }
            else
            {
                var normalBlob = normalOrdersContainer.GetBlockBlobReference($"{orderData.OrderTrackingId}.json");
                await normalBlob.UploadTextAsync(JsonConvert.SerializeObject(serializedOrderData));
            }
        }
    }
}