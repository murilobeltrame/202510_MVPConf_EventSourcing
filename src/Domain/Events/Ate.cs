using Domain.ValueObjects;

namespace Domain.Events;

public record Ate : IEvent
{
    public string Name { get; init; }
    public string Food { get; init; }
    public int? Quantity { get; init; }

    public Ate(string name, Food food, int? quantity = 1)
    {
        Name = name;
        Food = food.ToString();
        Quantity = quantity;
    }
    
    public Ate(string name, Prey prey, int? quantity = 1)
    {
        Name = name;
        Food = prey.ToString();
        Quantity = quantity;
    }
};