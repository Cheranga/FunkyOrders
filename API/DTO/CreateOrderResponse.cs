namespace API.DTO
{
    public class CreateOrderResponse
    {
        public CreateOrderResponse(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; }
    }
}