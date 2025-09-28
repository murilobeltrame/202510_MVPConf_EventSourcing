using Domain.ValueObjects;

namespace Domain.Events;

public record Hunted(string Name, Prey Prey) : IEvent;