using Marten;

namespace Infrastructure;

public class EventStore(IDocumentSession session): 
    Application.Abstractions.IEventStore
{
    public void Append<T>(T @event, string modifyingUser = "system")
        where T : Domain.Events.IEvent
    {
        session.LastModifiedBy = modifyingUser;
        session.Events.Append(@event.Name, @event);
    }
    
    public async Task SaveChangesAsync() => await session.SaveChangesAsync();
}