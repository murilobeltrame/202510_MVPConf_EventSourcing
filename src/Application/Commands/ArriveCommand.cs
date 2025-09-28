using Application.Abstractions;

using Domain.Events;
using Domain.ValueObjects;

namespace Application.Commands;
public record ArriveCommand(string Name, string SpeciesName) : ICommand
{
    public IEvent ToEvent()
    {
        var species = Species.List.FirstOrDefault(x => x.Name == SpeciesName)
            ?? throw new ArgumentOutOfRangeException(nameof(SpeciesName), $"Species '{SpeciesName}' is not recognized.");
        return new Arrived(Name, species);  
    } 
}
