using Application.Abstractions;

using Domain.Events;

namespace Application.Commands;

public record RestCommand(string Name) : ICommand
{
    public IEvent ToEvent() => new Rested(Name);
}
