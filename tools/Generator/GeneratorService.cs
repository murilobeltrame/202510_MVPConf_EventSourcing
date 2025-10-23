using Application.Abstractions;
using Application.Commands;

using AutoBogus;

using Domain.ValueObjects;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Generator;

public class GeneratorService(ICommandHandler handler, ILogger<GeneratorService> logger): IHostedService
{
    private bool _running = true;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (_running)
        {
            // get a list of names
            var arrivalsList = new AutoFaker<ArriveCommand>()
                .RuleFor(p => p.Name, f => f.Name.FirstName())
                .RuleFor(p => p.SpeciesName, f => f.PickRandom(Species.List).Name)
                .Generate(50);
            // for each name, generate an action order
            // should always start with an arrival and end with a departure
            // in between, there can be hunts and collects , explorations and rests on random order
            foreach (var arrival in arrivalsList)
            {
                logger.LogInformation("Generated arrival: {Name} the {Species}", arrival.Name, arrival.SpeciesName);
                await handler.Handle(arrival);
                
                var actionCount = new Random().Next(5, 15);
                logger.LogInformation("Action count: {ActionCount} for {Name}", actionCount, arrival.Name);
                
                for (int i = 0; i < actionCount; i++)
                {
                    var actionType = new Random().Next(0, 4);
                    switch (actionType)
                    {
                        case 0:
                            await handler.Handle(new ExploreCommand(arrival.Name));
                            break;
                        case 1:
                            await handler.Handle(new AutoFaker<CollectCommand>()
                                .RuleFor(p => p.Name, arrival.Name)
                                .RuleFor(p=> p.FoodName, f => f.PickRandom(Enum.GetNames<Food>()))
                                .RuleFor(p => p.Quantity, f => f.Random.Number(1, 10))
                                .Generate());
                            break;
                        case 2:
                            await handler.Handle(new AutoFaker<HuntCommand>()
                                .RuleFor(p => p.Name, arrival.Name)
                                .RuleFor(p => p.PreyName, f => f.PickRandom(Enum.GetNames<Prey>()))
                                .Generate());
                            break;
                        case 3:
                            await handler.Handle(new AutoFaker<EatCommand>()
                                .RuleFor(p => p.Name, arrival.Name)
                                .RuleFor(p => p.FoodName, 
                                    f => f.PickRandom( Enum.GetNames<Food>().Concat(Enum.GetNames<Prey>())))
                                .RuleFor(p => p.Quantity, f => f.Random.Number(1, 3))
                                .Generate());
                            break;
                        case 4:
                            await handler.Handle(new RestCommand(arrival.Name));
                            break;
                    }
                }
                
                logger.LogInformation("Generated departure: {Name} the {Species}", arrival.Name, arrival.SpeciesName);
                await handler.Handle(new DepartCommand(arrival.Name));
            }

            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _running = false;
        return Task.CompletedTask;
    } 
}