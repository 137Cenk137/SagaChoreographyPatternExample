using Shared.Events.Abstract;

namespace Shared.Events;

public class PaymentCompletedEvent : IEvent
{
    public Guid OrderId { get; set; }
}