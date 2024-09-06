using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<StockAPIDBContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnectionString"));
});
builder.Services.AddMassTransit(cfg => 
{
    cfg.UsingRabbitMq((context,_configure)=>{
        _configure.Host(builder.Configuration["RabbitMQ_URL"]);
    });
});
var app = builder.Build();



app.Run();
