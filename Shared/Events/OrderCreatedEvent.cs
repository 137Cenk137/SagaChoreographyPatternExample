using Shared.Events.Abstract;
using Shared.Messages;

namespace Shared.Events;

public class OrderCreatedEvent : IEvent
{
    public Guid OrderId { get; set; }
    public  Guid BuyerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal TotalPrice { get; set; }

    public IList<OrderItemMessage> OrderItemMessages { get; set; }

    
}