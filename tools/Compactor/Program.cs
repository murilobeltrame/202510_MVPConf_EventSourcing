using Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Projetor;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDataSource("ApplicationDb");
builder.AddServiceDefaults();

builder.Services
    .AddMartenDb()
    .UseNpgsqlDataSource();

builder.Services.AddHostedService<CompactStreams>();

var app = builder.Build();
await app.RunAsync();