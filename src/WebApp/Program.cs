using Application.Abstractions;
using Application.Commands;

using Infrastructure;

using WebApp;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureNpgsqlDataSource("ApplicationDb");
builder.AddServiceDefaults();

builder.Services
    .AddTransient<CorrelationIdMiddleware>()
    .AddTransient<ICommandHandler, CommandHandler>()
    .AddOpenApi()
    .AddMartenDb()
    .UseNpgsqlDataSource();

var app = builder.Build();

app
    .UseMiddleware<CorrelationIdMiddleware>()
    .UseHttpsRedirection();

app
    .MapWriteEndpoints()
    .MapReadEndpoints()
    .MapOpenApi();

await app.RunAsync();