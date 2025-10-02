namespace Application.Abstractions;
public interface ICommandHandler
{
    Task Handle<T>(T command) where T : ICommand;
}