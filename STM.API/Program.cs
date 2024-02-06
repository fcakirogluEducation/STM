using MassTransit;
using STM.API.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<BackgroundServiceOne>();

builder.Services.AddMassTransit(configure =>
{
    configure.UsingRabbitMq((context, configure) =>
    {
        configure.Durable = true;

        var connection = builder.Configuration.GetConnectionString("RabbitMQContainer");
        configure.Host(new Uri($"rabbitmq://{connection}"), hostOptions =>
        {
            hostOptions.Username("guest");
            hostOptions.Password("guest");
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();