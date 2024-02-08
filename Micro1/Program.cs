using Micro1.Models;
using Micro1.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<Micro2Services>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetSection("MicroservicesUrl")["Micro2"]!);
}).AddPolicyHandler(GetCircuitBreakerPolicy()).AddPolicyHandler(Retry());


static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(2, TimeSpan.FromSeconds(15));
}

static IAsyncPolicy<HttpResponseMessage> GetAdvancedCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError().AdvancedCircuitBreakerAsync(
            failureThreshold: 0.5,
            samplingDuration: TimeSpan.FromSeconds(10),
            minimumThroughput: 20,
            durationOfBreak: TimeSpan.FromSeconds(30));
}

static IAsyncPolicy<HttpResponseMessage> Retry()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(3, x => TimeSpan.FromSeconds(3));
}


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
        sqlOptions =>
        {
            /*sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);*/
        });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    appDbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();