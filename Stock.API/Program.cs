using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Consumer;
using Stock.API.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<StockAPIDBContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnectionString"));
});
builder.Services.AddMassTransit(cfg => 
{
    cfg.AddConsumer<OrderCreatedEventConsumer>();
    cfg.UsingRabbitMq((context,_configure)=>{
        _configure.Host(builder.Configuration["RabbitMQ_URL"]);

        _configure.ReceiveEndpoint(RebbitMQSettings.Stock_OrderCreatedEventQueue,e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});
var app = builder.Build();



app.Run();
