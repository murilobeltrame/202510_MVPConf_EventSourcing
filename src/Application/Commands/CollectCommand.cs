using Application.Abstractions;

using Domain.Events;
using Domain.ValueObjects;

namespace Application.Commands;
public record CollectCommand(string Name, string FoodName, int Quantity = 1) : ICommand
{
    public IEvent ToEvent() => Enum.TryParse(FoodName, true, out Food food) ?
        new Collected(Name, food, Quantity):
        throw new ArgumentException($"Invalid food name: {FoodName}");
}