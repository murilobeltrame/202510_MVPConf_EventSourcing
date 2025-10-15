using Domain.Events;

using JasperFx.Events;

using Marten.Events.Aggregation;

namespace Infrastructure.Projections;

public record Hunt(string Prey, int Quantity = 1);

public record Forage(string Food, int Quantity = 1);

public record Eat(string Food, int Quantity = 1);

public record AnimalSummary(
    string Name, 
    string Species,
    DateTime ArrivedAt,
    DateTime? LeftAt = null,
    TimeSpan? ExplorationDuration = null,
    TimeSpan? RestingDuration = null,
    IEnumerable<Hunt>? Hunts = null, 
    IEnumerable<Forage>? Foraging = null,
    IEnumerable<Eat>? Eats = null);

public class AnimalSummaryProjection: SingleStreamProjection<AnimalSummary, string>
{
    public AnimalSummary Create(IEvent<Arrived> @event)
    {
      var arrived = @event.Data as Arrived;
      return new AnimalSummary(arrived.Name, arrived.Species.Name, @event.Timestamp.LocalDateTime);
    }
    
    public AnimalSummary Apply(IEvent<Departed> @event, AnimalSummary summary) => 
        summary with { LeftAt = @event.Timestamp.LocalDateTime };

    public AnimalSummary Apply(Hunted @event, AnimalSummary summary)
    {
        var hunt = new Hunt(@event.Prey.ToString());
        if (summary.Hunts is null) return summary with { Hunts = [hunt] };
        if (summary.Hunts.All(w => w.Prey != hunt.Prey)) return summary with { Hunts = [..summary.Hunts, hunt] }; 
        var hunts = summary.Hunts.Select(w => w.Prey == hunt.Prey ? w with { Quantity = w.Quantity + 1 } : w);
        return summary with { Hunts = hunts };
    }
    
    public AnimalSummary Apply(Collected @event, AnimalSummary summary)
    {
        var forage = new Forage(@event.Food.ToString(), @event.Quantity ?? 1);
        if (summary.Foraging is null) return summary with { Foraging = [forage] };
        if (summary.Foraging.All(w => w.Food != forage.Food)) return summary with { Foraging = [..summary.Foraging, forage] }; 
        var foraging = summary.Foraging.Select(w => w.Food == forage.Food ? w with { Quantity = w.Quantity + @event.Quantity ?? 1 } : w);
        return summary with { Foraging = foraging };
    }

    public AnimalSummary Apply(Ate @event, AnimalSummary summary)
    {
        var eat = new Eat(@event.Food, @event.Quantity ?? 1);
        if (summary.Eats is null) return summary with { Eats = [eat] };
        if (summary.Eats.All(w => w.Food != eat.Food)) return summary with { Eats = [..summary.Eats, eat] }; 
        var eats = summary.Eats.Select(w => w.Food == eat.Food ? w with { Quantity = w.Quantity + @event.Quantity ?? 1 } : w);
        return summary with { Eats = eats };
    }

    public AnimalSummary Apply(IEvent<Explored> @event, AnimalSummary summary) => summary with{ ExplorationDuration = summary.ExplorationDuration + TimeSpan.FromHours(1) };
    
    public AnimalSummary Apply(IEvent<Rested> @event, AnimalSummary summary) => summary with{RestingDuration = summary.RestingDuration + TimeSpan.FromHours(1) };
}