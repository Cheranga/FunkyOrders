using System;
using API.DTO;
using API.Events;
using API.Util;
using Xunit;

namespace API.Tests.Unit
{
    public class OrderProcessorTests
    {
        [Fact]
        public void For_Invalid_Data_An_Exception_Will_Be_Thrown()
        {
            var orderProcessor = new OrderProcessor();

            Assert.Throws<ArgumentNullException>(() => orderProcessor.GetOrderNature(new OrderReceivedEvent(Guid.NewGuid(), null)));
            Assert.Throws<ArgumentNullException>(() => orderProcessor.GetOrderNature(null));
        }

        [Fact]
        public void If_Customer_Id_Is_Even_The_Order_Will_Be_VIP()
        {
            var orderProcessor = new OrderProcessor();
            var orderType = orderProcessor.GetOrderNature(new OrderReceivedEvent(Guid.NewGuid(), new CreateOrderRequest
            {
                CustomerId = 2
            }));

            Assert.Equal(OrderType.Vip, orderType);
        }

        [Fact]
        public void If_Customer_Id_Is_Odd_The_Order_Will_Be_Normal()
        {
            var orderProcessor = new OrderProcessor();
            var orderType = orderProcessor.GetOrderNature(new OrderReceivedEvent(Guid.NewGuid(), new CreateOrderRequest
            {
                CustomerId = 1
            }));

            Assert.Equal(OrderType.Normal, orderType);
        }
    }
}