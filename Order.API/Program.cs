using MassTransit;
using MassTransit.SqlTransport;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Enums;
using Order.API.Models;
using Order.API.Models.Context;
using Order.API.ViewModels;
using Shared;
using Shared.Events;
using Shared.Messages;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(cfg => 
{
    cfg.AddConsumer<PaymentFailedEventConsumer>();
    cfg.AddConsumer<PaymentCompletedEventConsumer>();
    cfg.AddConsumer<StockNotReservedEventConsumer>();
    cfg.UsingRabbitMq((context,_configure)=>{
        _configure.Host(builder.Configuration["RabbitMQ_URL"]);
        _configure.ReceiveEndpoint(RebbitMQSettings.Order_StockNotReservedEventQueue ,e => e.ConfigureConsumer<StockNotReservedEventConsumer>(context));
        _configure.ReceiveEndpoint(RebbitMQSettings.Order_PaymentCompletedEventQueue,e =>e.ConfigureConsumer<PaymentCompletedEventConsumer>(context));
        _configure.ReceiveEndpoint(RebbitMQSettings.Order_PaymentFailedEventQueue,e =>e.ConfigureConsumer<PaymentFailedEventConsumer>(context));

    });
});
builder.Services.AddDbContext<OrderAPIDBContext>(opt =>{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnectionString"));
});






var app = builder.Build();
//apı key ler environment ta tutuluyor ne demek
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/create-order",async (CreateOrderVM orderVM,OrderAPIDBContext _dbContext,IPublishEndpoint publishEndpoint)=>
{
    Order.API.Models.Order order = new()
    {
        BuyerId = Guid.TryParse(orderVM.BuyerId,out Guid _buyerId) ? _buyerId : Guid.NewGuid(),
        OrderItems = orderVM.OrderItems.Select(oi => new OrderItem(){
            Count = oi.Count,
            Price = oi.Price,
            ProductId = Guid.Parse(oi.ProductId)
        }).ToList(),
        OrderStatus = OrderStatus.Suspend,
        TotalPrice = orderVM.OrderItems.Sum(x => x.Price * x.Count),//iliskiler üzerinde ogrenebiliriz ama kolona yazdirmak daha az maliyetli olur
        CreatedDate = DateTime.UtcNow,
    };
    await _dbContext.Orders.AddAsync(order);
    await _dbContext.SaveChangesAsync();
    OrderCreatedEvent  orderCreatedEvent = new(){
        OrderId = order.Id,
        BuyerId = order.BuyerId,
        TotalPrice = order.TotalPrice,
        CreatedDate = DateTime.UtcNow,
        OrderItemMessages  = order.OrderItems.Select(x => new OrderItemMessage(){ProductId = x.ProductId,Count = x.Count,Price =x.Price}).ToList()
    }; 

    await publishEndpoint.Publish(orderCreatedEvent);

 
});  


app.Run();
