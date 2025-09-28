using Domain.Events;

namespace Application.Abstractions;

public interface ICommand
{
    IEvent ToEvent();
}