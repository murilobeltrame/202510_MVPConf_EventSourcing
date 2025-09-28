using JasperFx.Events;

using Marten;

using Microsoft.Extensions.DependencyInjection;

using IEventStore = Application.Abstractions.IEventStore;

namespace Infrastructure;

public static class ServiceProviderExtensions
{
    public static MartenServiceCollectionExtensions.MartenConfigurationExpression AddMartenDb(this IServiceCollection services)
    {
        return services
            .AddTransient<IEventStore, EventStore>()
            .AddMarten(o =>
            {
                o.Events.StreamIdentity = StreamIdentity.AsString;
            });
    }
}