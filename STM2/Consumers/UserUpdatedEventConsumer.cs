using MassTransit;
using Shared.Events.Events;

namespace STM2.API.Consumers
{
    public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
    {
        public Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            Console.WriteLine($"Update User :{context.Message.UserName}");

            return Task.CompletedTask;
        }
    }
}