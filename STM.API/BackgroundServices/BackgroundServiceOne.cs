namespace STM.API.BackgroundServices
{
    public class BackgroundServiceOne : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("BackgroundServiceOne çalıştı");

            return Task.CompletedTask;
        }
    }
}