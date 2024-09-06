using Shared.Events.Abstract;

namespace Shared.Events;

public class StockNotReseredEvent : IEvent
{
    public Guid BuyerId { get; set; }
    public Guid OrderId { get; set;}
    public string Messages { get; set; }
}