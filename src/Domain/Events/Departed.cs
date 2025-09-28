namespace Domain.Events;

public record Departed(string Name) : IEvent;