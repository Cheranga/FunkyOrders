using System;

namespace API.DTO
{
    public class CreateOrderRequest
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public int CustomerId { get; set; }
    }
}