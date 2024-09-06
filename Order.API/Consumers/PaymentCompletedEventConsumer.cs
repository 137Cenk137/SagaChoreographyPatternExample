using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Enums;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers;

public class PaymentCompletedEventConsumer(OrderAPIDBContext _dbcontext) : IConsumer<PaymentCompletedEvent>
{
    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        Models.Order? order =  await _dbcontext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);//null olup olmadigini kontrol et
        if(order is null)
            throw new NullReferenceException();
        order.OrderStatus = OrderStatus.Completed;
        await _dbcontext.SaveChangesAsync();
    }
}