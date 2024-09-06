using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers;

public class PaymentFailedEventConsumer(OrderAPIDBContext _dbcontext) : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        Models.Order? order =  await _dbcontext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);//null olup olmadigini kontrol et
         if(order is null)
            throw new NullReferenceException();
        order.OrderStatus = Enums.OrderStatus.Fail;
        await _dbcontext.SaveChangesAsync();
    }
}