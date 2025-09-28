namespace Domain.Events;

public interface IEvent
{
    string Name { get; init; }
}