using System;
using API.DTO;

namespace API.Events
{
    public class OrderReceivedEvent
    {
        public OrderReceivedEvent(Guid orderId, CreateOrderRequest originalOrder)
        {
            OrderId = orderId;
            OriginalOrder = originalOrder ?? throw new ArgumentNullException(nameof(originalOrder));

            OrderTrackingId = $"{originalOrder.CustomerId}_{orderId}";
        }

        public Guid OrderId { get; }
        public CreateOrderRequest OriginalOrder { get; }
        public string OrderTrackingId { get; }
    }
}