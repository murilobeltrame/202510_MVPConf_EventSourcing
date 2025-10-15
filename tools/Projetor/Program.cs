using Infrastructure;

using JasperFx.Events.Daemon;

using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDataSource("ApplicationDb");
builder.AddServiceDefaults();

builder.Services
    .AddMartenDb()
    .UseNpgsqlDataSource()
    .AddAsyncDaemon(DaemonMode.HotCold);
    
var app = builder.Build();
    
await app.RunAsync();