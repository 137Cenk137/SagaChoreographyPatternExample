using Shared.Events.Abstract;
using Shared.Messages;

namespace Shared.Events;
public class PaymentFailedEvent : IEvent
{
    public Guid OrderId { get; set; }
    public string Message { get; set; }
    public IList<OrderItemMessage> orderItemMessages { get; set; }
}