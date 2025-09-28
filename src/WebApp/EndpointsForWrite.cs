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
        [FromServices] ICommandHandler<DepartCommand> handler,
        [FromBody] DepartCommand command) => 
        await handler.Handle(command);

    private static async Task HandleRests(
        [FromServices] ICommandHandler<RestCommand> handler,
        [FromBody] RestCommand command) => 
        await handler.Handle(command);

    private static async Task HandleFeedings(
        [FromServices] ICommandHandler<EatCommand> handler,
        [FromBody] EatCommand command) => 
        await handler.Handle(command);

    private static async Task HandleHunts(
        [FromServices] ICommandHandler<HuntCommand> handler,
        [FromBody] HuntCommand command) => 
        await handler.Handle(command);

    private static async Task HandleCollections(
        [FromServices] ICommandHandler<CollectCommand> handler,
        [FromBody] CollectCommand command) => 
        await handler.Handle(command);

    private static async Task HandleExplorations(
        [FromServices] ICommandHandler<ExploreCommand> handler,
        [FromBody] ExploreCommand command) => 
        await handler.Handle(command);

    private static async Task HandleArrivals(
        [FromServices] ICommandHandler<ArriveCommand> handler,
        [FromBody] ArriveCommand command) => 
        await handler.Handle(command);
}