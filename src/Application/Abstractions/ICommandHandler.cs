namespace Application.Abstractions;
public interface ICommandHandler
{
    Task Handle<T>(T command, string? modifyingUser = null) where T : ICommand;
}