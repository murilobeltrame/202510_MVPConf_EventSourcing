using Projects;

var builder = DistributedApplication.CreateBuilder(args);

const string databaseName = "ApplicationDb";
var database = builder.AddAzurePostgresFlexibleServer("database-server")
    .RunAsContainer(b => b
        .WithEnvironment("POSTGRES_DB", databaseName)
        .WithPgWeb())
    .AddDatabase(databaseName);

var broker = builder.AddRabbitMQ("broker")
    .WithManagementPlugin();

builder.AddProject<WebApp>("webapp")
    .WithReference(database).WaitFor(database)
    .WithReference(broker).WaitFor(broker);

await builder.Build().RunAsync();
