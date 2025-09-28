using Marten;

namespace Infrastructure;

public class EventStore(IDocumentSession session): 
    Application.Abstractions.IEventStore
{
    public void Append<T>(T @event)
        where T : Domain.Events.IEvent
        => session.Events.Append(@event.Name, @event);
    public async Task SaveChangesAsync() => await session.SaveChangesAsync();
}