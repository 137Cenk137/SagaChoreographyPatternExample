using MassTransit;
using Shared;
using Shared.Events;

namespace Payment.API.Consumer;

public class StockReservedEventConsumer(IPublishEndpoint _publishEndpoint,ISendEndpointProvider _sendEndpointProvider) : IConsumer<StockReservedEvent>
{
    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
       if(true)
       {

        //odeme basarılı...
        PaymentCompletedEvent paymentCompletedEvent = new(){
            OrderId = context.Message.OrderId,
        };
        await _publishEndpoint.Publish(paymentCompletedEvent);
        await Console.Out.WriteLineAsync("Odeme basarili");
       }
       else
       {
        //odeme basarısız...
        PaymentFailedEvent paymentFailedEvent = new(){
            OrderId = context.Message.OrderId,
            Message = "cash problem",
            orderItemMessages = context.Message.orderItemMessages,
            
        };
        await _publishEndpoint.Publish(paymentFailedEvent);
        await Console.Out.WriteLineAsync("Odeme basarisiz");


       }
    }
}