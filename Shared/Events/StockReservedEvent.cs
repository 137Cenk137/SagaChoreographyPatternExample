using Shared.Events.Abstract;
using Shared.Messages;

namespace Shared.Events;

public class StockReservedEvent : IEvent
{
    public Guid BuyerId { get; set; }
    public Guid OrderId { get; set;}
    public decimal TotalPrice { get; set; }
    public List<OrderItemMessage> orderItemMessages{ get; set; } 
}