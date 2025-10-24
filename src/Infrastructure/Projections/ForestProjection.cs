using Domain.Events;

using Marten.Events.Aggregation;

namespace Infrastructure.Projections;

public record Animal(string Name);

public class ForestProjection: SingleStreamProjection<Animal, string>
{
    public ForestProjection()
    {
        CreateEvent<Arrived>(e => new Animal(e.Name));
        DeleteEvent<Departed>();
    }
}