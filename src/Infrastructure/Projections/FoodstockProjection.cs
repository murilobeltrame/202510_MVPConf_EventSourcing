using Domain.Events;

using Marten.Events.Projections;

namespace Infrastructure.Projections;

public record Foodstock(string FoodItem, int Quantity);

public class FoodstockProjection: MultiStreamProjection<Foodstock, string>
{
    public FoodstockProjection()
    {
        Identity<Hunted>(e => e.Prey.ToString());
        Identity<Collected>(e => e.Food.ToString());
        Identity<Ate>(e => e.Food);
        
        CreateEvent<Hunted>(e => new Foodstock(e.Prey.ToString(), 1));
        CreateEvent<Collected>(e => new Foodstock(e.Food.ToString(), e.Quantity ?? 1));
    }
    
    public static Foodstock Apply(Hunted _, Foodstock stock) =>
        stock with { Quantity = stock.Quantity + 1 };
    public static Foodstock Apply(Collected @event, Foodstock stock) =>
        stock with { Quantity = stock.Quantity + (@event.Quantity ?? 1) };
    
    public static Foodstock Apply(Ate @event, Foodstock stock) =>
        stock with { Quantity = stock.Quantity - (@event.Quantity ?? 1) };
}