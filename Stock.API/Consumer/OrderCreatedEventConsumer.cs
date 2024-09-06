
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.API.Migrations;
using Stock.API.Models.Contexts;

namespace Stock.API.Consumer;

public class OrderCreatedEventConsumer(StockAPIDBContext _dbcontext,IPublishEndpoint _publishEndpoint,ISendEndpointProvider _sendEndpointProvider) : IConsumer<OrderCreatedEvent>
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
            StockReservedEvent stockReservedEvent = new(){
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                TotalPrice = context.Message.TotalPrice,
                orderItemMessages = context.Message.OrderItemMessages
            };
            await _publishEndpoint.Publish(stockReservedEvent);
        }
        else 
        {
            StockNotReseredEvent stockNotReseredEvent = new()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                Messages = "There is no enough product  in stock "
            };
            await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{Shared.RebbitMQSettings.Order_StockNotReservedEventQueue}"));
            await _sendEndpointProvider.Send(stockNotReseredEvent);
            //stock islemleri basarisiz
            //order api uyarıcak event fırlatılacak
        }
    }
      
}