namespace Shared.Events.Events
{
    public record UserCreatedEvent
    {
        public string UserName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Phone { get; init; } = null!;
    }
}