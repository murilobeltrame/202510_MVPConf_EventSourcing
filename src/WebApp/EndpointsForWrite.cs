using Application.Abstractions;
using Application.Commands;

using ImTools;

using Microsoft.AspNetCore.Mvc;

namespace WebApp;

public static class EndpointsForWrite
{
    public static IEndpointRouteBuilder MapWriteEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/arrivals", HandleArrivals);
        app.MapPost("/explorations", HandleExplorations);
        app.MapPost("/foragings", HandleCollections);
        app.MapPost("/hunts", HandleHunts);
        app.MapPost("/feedings", HandleFeedings);
        app.MapPost("/rests", HandleRests);
        app.MapPost("/departures", HandleDepartures);

        return app;
    }

    private static async Task HandleDepartures(
        [FromServices] ICommandHandler handler,
        [FromBody] DepartCommand command) => 
        await handler.Handle(command, "apiUser");

    private static async Task HandleRests(
        [FromServices] ICommandHandler handler,
        [FromBody] RestCommand command) => 
        await handler.Handle(command, "apiUser");

    private static async Task HandleFeedings(
        [FromServices] ICommandHandler handler,
        [FromBody] EatCommand command) => 
        await handler.Handle(command, "apiUser");

    private static async Task HandleHunts(
        [FromServices] ICommandHandler handler,
        [FromBody] HuntCommand command) => 
        await handler.Handle(command, "apiUser");

    private static async Task HandleCollections(
        [FromServices] ICommandHandler handler,
        [FromBody] CollectCommand command) => 
        await handler.Handle(command, "apiUser");

    private static async Task HandleExplorations(
        [FromServices] ICommandHandler handler,
        [FromBody] ExploreCommand command) => 
        await handler.Handle(command, "apiUser");

    private static async Task HandleArrivals(
        [FromServices] ICommandHandler handler,
        [FromBody] ArriveCommand command) => 
        await handler.Handle(command, "apiUser");
}