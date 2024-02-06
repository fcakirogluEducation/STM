using MassTransit;
using Shared.Events.Events;

namespace STM2.Consumers
{
    public class UserCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<UserCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            Console.WriteLine($"Hata fırlatılmadan önce");
            //throw new DivideByZeroException("hat var");


            // db  işlemi
            // 
            // publishEndpoint.Publish()
            // 2. işlem
            // publishEndpoint.Publish()


            return Task.CompletedTask;
        }
    }
}