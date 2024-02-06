using MassTransit;
using STM2.API.Consumers;
using STM2.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<UserCreatedEventConsumer>();
    configure.AddConsumer<UserUpdatedEventConsumer>();

    configure.UsingRabbitMq((context, configure) =>
    {
        //configure.PrefetchCount = 10;

        //1. step
        //configure.UseMessageRetry(x => x.Immediate(5));


        //configure.UseMessageRetry(x => { x.Handle<DivideByZeroException>(); });
        configure.UseMessageRetry(x => x.Incremental(5, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(2)));


        //2. step
        //configure.UseDelayedRedelivery(configureRetry => { configureRetry.Interval(5, TimeSpan.FromMinutes(20)); });
        //3. step
        configure.UseInMemoryOutbox(context);

        var connection = builder.Configuration.GetConnectionString("RabbitMQContainer");
        configure.Host(new Uri($"rabbitmq://{connection}"), hostOptions =>
        {
            hostOptions.Username("guest");
            hostOptions.Password("guest");
        });

        configure.ReceiveEndpoint("stm2.user.created.event.queue",
            receiveOptions =>
            {
                //receiveOptions.ConcurrentMessageLimit = 1;
                receiveOptions.ConfigureConsumer<UserCreatedEventConsumer>(context);
            });

        configure.ReceiveEndpoint("stm2.updated.user.queue",
            receiveOptions =>
            {
                //receiveOptions.ConcurrentMessageLimit = 1;
                receiveOptions.ConfigureConsumer<UserUpdatedEventConsumer>(context);
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