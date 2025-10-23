namespace Application.Abstractions;

public interface IEventStore
{
    void Append<T>(T @event, string modifyingUser) where T : Domain.Events.IEvent;
    Task SaveChangesAsync();
}