using Application.Abstractions;

using Domain.Events;
using Domain.ValueObjects;

namespace Application.Commands;
public record EatCommand(string Name, string FoodName, int Quantity = 1) : ICommand
{
    public IEvent ToEvent()
    {
        if(Enum.TryParse(FoodName, true, out Food food))
            return new Ate(Name, food, Quantity);  
        if(Enum.TryParse(FoodName, true, out Prey prey))
            return new Ate(Name, prey, Quantity);
        throw new ArgumentException($"Invalid food or prey name: {FoodName}");
    } 
}
