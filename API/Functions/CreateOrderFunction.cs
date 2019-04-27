using System;
using System.IO;
using System.Threading.Tasks;
using API.DTO;
using API.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Functions
{
    public static class CreateOrderFunction
    {
        [FunctionName("CreateOrder")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            [Queue("orders")] IAsyncCollector<OrderReceivedEvent> ordersQueue,
            ILogger log)
        {
            log.LogInformation("Received a create order request");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return new BadRequestObjectResult("Request is empty");
            }

            log.LogInformation("Request body");
            log.LogInformation(requestBody);


            var request = JsonConvert.DeserializeObject<CreateOrderRequest>(requestBody);
            if (request == null)
            {
                return new BadRequestObjectResult("Invalid request format");
            }

            var orderReceived = new OrderReceivedEvent(Guid.NewGuid(), request);

            await ordersQueue.AddAsync(orderReceived);

            log.LogInformation("Inserted the order created event to the queue");


            var response = new CreateOrderResponse(orderReceived.OrderTrackingId);

            return new OkObjectResult(response);
        }
    }
}