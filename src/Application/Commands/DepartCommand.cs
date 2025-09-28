using Application.Abstractions;

using Domain.Events;

namespace Application.Commands;
public record DepartCommand(string Name) : ICommand
{
    public IEvent ToEvent() => new Departed(Name);
}