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
        // GET all animals who arrived
        app.MapGet("/arrivals", ( 
            HttpResponse response,
            [FromServices] IDocumentSession session,
            [FromQuery] uint page = 0, 
            [FromQuery] ushort size = 10) =>
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
            [FromQuery] ushort size = 10) =>
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
            [FromQuery] ushort size = 10) =>
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
        
        // GET all animals currently in the forest
        app.MapGet("/v2/animals", (
            HttpResponse response, 
            [FromServices] IDocumentSession session, 
            [FromQuery] uint page = 0, 
            [FromQuery] ushort size = 10) =>
        {
            var result = session.Query<Animal>();
            response.Headers.Append("X-Total-Count", result.Count().ToString());
            return Results.Ok(result.Skip((int)page * size).Take(size));
        });

        app.MapGet("/meals/{food}", (
            HttpResponse response,
            [FromServices] IDocumentSession session,
            [FromRoute] string food,
            [FromQuery] uint page = 0,
            [FromQuery] ushort size = 10) =>
        {
            var result = session
                .Events
                .QueryRawEventDataOnly<Ate>()
                .Where(a => a.Food.ToLowerInvariant() == food.ToLowerInvariant())
                .Select(s => new { s.Name, s.Quantity });
            response.Headers.Append("X-Total-Count", result.Count().ToString());
            return Results.Ok(result.Skip((int)page * size).Take(size));
        });

        app.MapGet("/foodstock", (
            [FromServices] IDocumentSession session) => 
            session.Query<Foodstock>());

        app.MapGet("/animal/{name}", async (
            [FromServices] IDocumentSession session,
            [FromRoute] string name,
            [FromQuery] uint? untilDays) =>
        {
            var data = untilDays.HasValue
                ? await session.Events.FetchStreamAsync(name, timestamp: DateTime.UtcNow.AddDays(untilDays.Value * -1))
                : await session.Events.FetchStreamAsync(name);
            return data.Select(s => new { s.EventTypeName, s.Timestamp, s.Data });
        });
        
        return app;
    }
}