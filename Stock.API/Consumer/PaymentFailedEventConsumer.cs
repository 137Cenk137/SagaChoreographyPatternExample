using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.API.Models.Contexts;

namespace Stock.API.Consumer;

public class PaymentFailedEventConsumer(StockAPIDBContext _dbcontext) : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var stocks = await _dbcontext.Stocks.ToListAsync();
        foreach (var item in context.Message.orderItemMessages)
        {
            var stock = await _dbcontext.Stocks.SingleOrDefaultAsync(x=> x.ProductId == item.ProductId);
            if (stock != null)
            {
                stock.Count += item.Count;
            }

        
        }
        await _dbcontext.SaveChangesAsync();
    }
}