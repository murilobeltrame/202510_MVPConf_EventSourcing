using Domain.ValueObjects;

namespace Domain.Events;

public record Collected(string Name, Food Food, int? Quantity = 1): IEvent;