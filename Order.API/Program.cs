using MassTransit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(cfg => 
{
    cfg.UsingRabbitMq((context,_configure)=>{
        _configure.Host(builder.Configuration["RabbitMQ_URL"]);
    });
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
