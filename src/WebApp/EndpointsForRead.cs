using Domain.Events;

using Infrastructure.Projections;

using Marten;
using Marten.Events;

using Microsoft.AspNetCore.Mvc;

namespace WebApp;

public static class EndpointsForRead
{
    public static IEndpointRouteBuilder MapReadEndpoints(this IEndpointRouteBuilder app)
    {
        // app.MapGet("/arrivals", HandleArrivals);
        app.MapGet("/hunts", HandleHunts);
        app.MapGet("/animal/{name}/actions", HandleFetchActionsForAnimalByName);
        app.MapGet("/animal/{name}", HandleGetSummaryForAnimalByName);
        app.MapGet("v2/animal/{name}", HandleGetSummaryForAnimalByNameV2);
        app.MapGet("v2/animal/{name}:until", HandleGetSummaryForAnimalByNameAndTimeV2);
        app.MapGet("v2/animal", HandleGetSummaryForAnimalsV2);
        // app.MapGet("/animals/{name}/actions", (string name) => Results.Ok($"Get all actions for animal: {name}"));
        // app.MapGet("/animals-with-food-and-hunts", () => Results.Ok("Get all animals, food and hunts"));
        // app.MapGet("/animals-with-food-and-hunts-since/{days}", (int days) => Results.Ok($"Get all animals, food and hunts in the last {days} days"));

        // GET all animals who arrived
        app.MapGet("/arrivals", ( 
            HttpResponse response,
            [FromServices] IDocumentSession session,
            [FromQuery] uint page = 0, 
            [FromQuery] ushort size = 100) =>
        {
            var result = session
                .Events
                .QueryRawEventDataOnly<Arrived>()
                .Select(s => new { s.Name, Species = s.Species.Name })
                .Distinct();
            response.Headers.Append("X-Total-Count", result.Count().ToString());
            return Results.Ok(result.Skip((int)page * size).Take(size));
        });
        
        // GET all animals who departed
        app.MapGet("/departures", (
            HttpResponse response, 
            [FromServices] IDocumentSession session, 
            [FromQuery] uint page = 0, 
            [FromQuery] ushort size = 100) =>
        {
            var result = session
                .Events
                .QueryRawEventDataOnly<Departed>()
                .Select(s => new { s.Name })
                .Distinct();
            response.Headers.Append("X-Total-Count", result.Count().ToString());
            return Results.Ok(result.Skip((int)page * size).Take(size));
        });
        
        // GET all animals currently in the forest
        app.MapGet("/animals", (
            HttpResponse response, 
            [FromServices] IDocumentSession session, 
            [FromQuery] uint page = 0, 
            [FromQuery] ushort size = 100) =>
        {
            var arrivals = session
                .Events
                .QueryRawEventDataOnly<Arrived>()
                .Select(s => new { s.Name, Species = s.Species.Name })
                .Distinct();
            var departures = session
                .Events
                .QueryRawEventDataOnly<Departed>()
                .Select(s => new { s.Name })
                .Distinct();
            var result = arrivals
                .Where(a => !departures.Any(d => d.Name == a.Name));
            response.Headers.Append("X-Total-Count", result.Count().ToString());
            return Results.Ok(result.Skip((int)page * size).Take(size));
        });
        
        // GET how much food has been collected for each animal
        // GET how much food has been collected OR hunted for each animal
        // GET how much food has been collected given some time in the past.
        // GET what every animal has eaten
        
        return app;
    }
    
    // Get all animals
    private static IResult HandleArrivals( 
        HttpResponse response,
        [FromServices] IDocumentSession session,
        [FromQuery] uint page = 0, 
        [FromQuery] ushort size = 100)
    {
        var result = session
            .Events
            .QueryRawEventDataOnly<Arrived>()
            .Select(s => new { s.Name, Species = s.Species.Name })
            .Distinct();
        response.Headers.Append("X-Total-Count", result.Count().ToString());
        return Results.Ok(result.Skip((int)page * size).Take(size));
    }
    
    // Get everytime an animal hunted
    private static async Task<IResult> HandleHunts( 
        HttpResponse response,
        [FromServices] IDocumentSession session,
        [FromQuery] uint page = 0, 
        [FromQuery] ushort size = 100)
    {
        var result = await session
            .Events
            .QueryAllRawEvents()
            .Where(e => e.EventTypesAre(typeof(Hunted)))
            .ToListAsync();
        response.Headers.Append("X-Total-Count", result.Count.ToString());
        return Results.Ok(result
            .Select(s => new
            {
                ((s.Data as Hunted)!).Name,
                Prey = ((s.Data as Hunted)!).Prey.ToString(),
                s.Timestamp
            })
            .Skip((int)page * size)
            .Take(size));
    }
    
    // Get all actions for a given animal
    private static async Task<IResult> HandleFetchActionsForAnimalByName(
        [FromServices] IDocumentSession session, 
        [FromRoute] string name)
    {
        var result = await session
            .Events
            .QueryAllRawEvents()
            .Where(e => e.StreamKey == name)
            .ToListAsync();
        return Results.Ok(result
            .Select(s => new
            {
                ((s.Data as IEvent)!).Name,
                s.EventTypeName,
                s.Timestamp
            }));
    }
    
    // Get a summary for a given animal
    private static async Task<IResult> HandleGetSummaryForAnimalByName(
        [FromServices] IDocumentSession session,
        [FromRoute] string name)
    {
        var result = await session
            .Events
            .QueryAllRawEvents()
            .Where(e => e.StreamKey == name)
            .ToListAsync();

        var arrived = result.Any(e => e.EventTypesAre(typeof(Arrived)));
        var departed = result.Any(e => e.EventTypesAre(typeof(Departed)));
        var hunts = result
            .Where(e => e.EventTypesAre(typeof(Hunted)))
            .Select(e => new
            {
                Prey = ((e.Data as Hunted)!).Prey.ToString(),
                e.Timestamp
            });
        var ate = result
            .Where(e => e.EventTypesAre(typeof(Ate)))
            .Select(e => new
            {
                ((e.Data as Ate)!).Food,
                ((e.Data as Ate)!).Quantity,
                e.Timestamp
            });

        return Results.Ok(new
        {
            Name = name,
            Arrived = arrived,
            Departed = departed,
            Hunts = hunts,
            Ate = ate
        });
    }

    // Better way to do the summary
    private static async Task<IResult> HandleGetSummaryForAnimalByNameV2(
        [FromServices] IQuerySession session,
        [FromRoute] string name)
    {
        var result = await session.Query<AnimalSummary>()
            .FirstOrDefaultAsync(a => a.Name == name);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
    
    // Get for a specific version
    private static async Task<IResult> HandleGetSummaryForAnimalByNameAndTimeV2(
        [FromServices] IDocumentSession session,
        [FromRoute] string name,
        [FromQuery] ushort minutes)
    {
        var offset = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(minutes);
        var result = await session.Events.AggregateStreamAsync<AnimalSummary>(name, timestamp: offset);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
    
    // Get a summary for all animals
    private static async Task<IResult> HandleGetSummaryForAnimalsV2(
        [FromServices] IQuerySession session,
        [FromQuery] uint page = 0,
        [FromQuery] ushort size = 100)
    {
        var result = await session.Query<AnimalSummary>()
            .Skip((int)page * size)
            .Take(size)
            .ToListAsync();
        return Results.Ok(result);
    }
    
    
    // Get all animals, food and hunts
    
    // Get all animals, food and hunts in a give time backward
}