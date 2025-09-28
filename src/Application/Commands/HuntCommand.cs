using Application.Abstractions;

using Domain.Events;
using Domain.ValueObjects;

namespace Application.Commands;

public record HuntCommand(string Name, string PreyName) : ICommand
{
    public IEvent ToEvent() => Enum.TryParse(PreyName,true, out Prey prey) ? 
            new Hunted(Name, prey) : 
            throw new ArgumentException($"Invalid prey name: {PreyName}");
}
