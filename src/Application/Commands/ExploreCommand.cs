using Application.Abstractions;

using Domain.Events;

namespace Application.Commands;
public record ExploreCommand(string Name) : ICommand
{
    public IEvent ToEvent() => new Explored(Name);
}