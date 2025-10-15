using Application.Abstractions;
using Application.Commands;

using Infrastructure;

using WebApp;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureNpgsqlDataSource("ApplicationDb");
builder.AddRabbitMQClient("broker");
builder.AddServiceDefaults();

builder.Services
    .AddTransient<ICommandHandler, CommandHandler>()
    .AddOpenApi()
    .AddMartenDb()
    .UseNpgsqlDataSource();

var app = builder.Build();

app.UseHttpsRedirection();

app
    .MapWriteEndpoints()
    .MapReadEndpoints()
    .MapOpenApi();

await app.RunAsync();