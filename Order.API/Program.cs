using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Enums;
using Order.API.Models;
using Order.API.Models.Context;
using Order.API.ViewModels;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(cfg => 
{
    cfg.UsingRabbitMq((context,_configure)=>{
        _configure.Host(builder.Configuration["RabbitMQ_URL"]);
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

app.MapPost("/create-order",async (CreateOrderVM orderVM,OrderAPIDBContext _dbContext)=>
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

});


app.Run();
