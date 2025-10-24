using Domain.Events;

using Infrastructure.Projections;

using JasperFx.Core;

using Marten;

using Microsoft.Extensions.Hosting;

namespace Projetor;

public class CompactStreams(IDocumentSession session): IHostedService
{
    private bool _running = true;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (_running)
        {
            await CompactForest();
            
            await CompactFoodstock();
            
            await Task.Delay(TimeSpan.FromDays(1), cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _running = false;
        return Task.CompletedTask;
    }

    private async Task CompactForest()
    {
        var streamIds = await session.Query<Animal>()
            .Select(x => x.Name)
            .ToListAsync();
        foreach (var streamId in streamIds)
        {
            await Compact<Animal>(streamId);
        }
    }
    
    private async Task CompactFoodstock() 
    {
        var streamIds = await session.Query<Foodstock>()
            .Select(x => x.FoodItem)
            .ToListAsync();
        foreach (var streamId in streamIds)
        {
            await Compact<Foodstock>(streamId);
        }
    }
    
    private async Task Compact<T>(string streamId) =>
        await session.Events.CompactStreamAsync<T>(streamId, c =>
            c.Timestamp = DateTimeOffset.UtcNow.Subtract(7.Days()));
}