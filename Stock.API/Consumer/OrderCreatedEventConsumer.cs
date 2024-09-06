
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.API.Models.Contexts;

namespace Stock.API.Consumer;

public class OrderCreatedEventConsumer(StockAPIDBContext _dbcontext) : IConsumer<OrderCreatedEvent>
{

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        List<bool> stockResult = new();
        List<Stock.API.Models.Stock> list  = await _dbcontext.Stocks.ToListAsync();
        
        foreach (var orderItemMessage in context.Message.OrderItemMessages)
        {
            stockResult.Add(list.Any(x => x.ProductId == orderItemMessage.ProductId && x.Count >= orderItemMessage.Count));
        }
        if(stockResult.TrueForAll(s => s.Equals(true)))
        {
            //Stock Güncellenmesi
            foreach (var item in context.Message.OrderItemMessages)
            {
                var stock = await _dbcontext.Stocks.SingleOrDefaultAsync(x => x.ProductId == item.ProductId);
                stock.Count -= item.Count;

            }
            await _dbcontext.SaveChangesAsync();
            //payment i firlatilacak event
        }
        else 
        {
            //stock islemleri basarisiz
            //order api uyarıcak event fırlatılacak
        }
    }
      
}