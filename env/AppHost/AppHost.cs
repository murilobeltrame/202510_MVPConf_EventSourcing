using Projects;

var builder = DistributedApplication.CreateBuilder(args);

const string databaseName = "ApplicationDb";
var database = builder.AddAzurePostgresFlexibleServer("database-server")
    .RunAsContainer(b => b
        .WithDataVolume()
        .WithEnvironment("POSTGRES_DB", databaseName)
        .WithPgWeb())
    .AddDatabase(databaseName);

builder.AddProject<WebApp>("webapp")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Generator>("generator")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projetor>("projector")
    .WithReference(database)
    .WaitFor(database);

await builder.Build().RunAsync();
