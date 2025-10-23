using Application.Abstractions;

namespace Application.Commands;
public class CommandHandler(IEventStore eventStore): ICommandHandler 
{
    public async Task Handle<T>(T command, string? modifyingUser) where T : ICommand
    {
        eventStore.Append(command.ToEvent(), modifyingUser ?? "system");
        await eventStore.SaveChangesAsync();
    }
}