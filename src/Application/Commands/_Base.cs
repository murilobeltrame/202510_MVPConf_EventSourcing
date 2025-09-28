using Application.Abstractions;

namespace Application.Commands;
public class CommandHandler<TCommand>(IEventStore eventStore): ICommandHandler<TCommand> 
    where TCommand : ICommand
{
    public async Task Handle(TCommand command)
    {
        eventStore.Append(command.ToEvent());
        await eventStore.SaveChangesAsync();
    }
}