using API.Events;

namespace API.Util
{
    public interface IOrderProcessor
    {
        OrderType GetOrderNature(OrderReceivedEvent data);
    }
}