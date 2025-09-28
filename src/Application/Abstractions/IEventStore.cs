namespace Application.Abstractions;

public interface IEventStore
{
    void Append<T>(T @event) where T : Domain.Events.IEvent;
    Task SaveChangesAsync();
}