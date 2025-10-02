using Application.Abstractions;
using Application.Commands;

using Generator;

using Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDataSource("ApplicationDb");
builder.AddServiceDefaults();

builder.Services
    .AddTransient<ICommandHandler, CommandHandler>()
    .AddMartenDb()
    .UseNpgsqlDataSource();
builder.Services.AddHostedService<GeneratorService>();

var app = builder.Build();
await app.RunAsync();