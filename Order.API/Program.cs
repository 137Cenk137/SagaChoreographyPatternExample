using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Context;

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




app.Run();
