using Domain.ValueObjects;

namespace Domain.Events;

public record Arrived(string Name, Species Species): IEvent;