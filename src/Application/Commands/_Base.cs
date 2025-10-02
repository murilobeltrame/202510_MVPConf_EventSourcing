using Application.Abstractions;

namespace Application.Commands;
public class CommandHandler(IEventStore eventStore): ICommandHandler 
{
    public async Task Handle<T>(T command) where T : ICommand
    {
        eventStore.Append(command.ToEvent());
        await eventStore.SaveChangesAsync();
    }
}