using MassTransit;
using Payment.API.Consumer;
using Shared;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddMassTransit(cfg => 
{
   cfg.AddConsumer<StockReservedEventConsumer>();
    cfg.UsingRabbitMq((context,_configure)=>{
        _configure.Host(builder.Configuration["RabbitMQ_URL"]);
        _configure.ReceiveEndpoint(RebbitMQSettings.Payment_StockReservedEventQueue,e => e.ConfigureConsumer<StockReservedEventConsumer>(context));
    });
});
var app = builder.Build();


app.Run();
