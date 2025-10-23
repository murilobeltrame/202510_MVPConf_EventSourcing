using Infrastructure.Projections;

using JasperFx.Events;
using JasperFx.Events.Projections;

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
                o.Events.MetadataConfig.HeadersEnabled = true;
                o.Events.MetadataConfig.CausationIdEnabled = true;
                o.Events.MetadataConfig.CorrelationIdEnabled = true;
                o.Events.MetadataConfig.UserNameEnabled = true;
                
                o.Events.StreamIdentity = StreamIdentity.AsString;
                
                o.Schema.For<AnimalSummary>().Identity(x => x.Name);
                
                o.Projections.Add<AnimalSummaryProjection>(ProjectionLifecycle.Async);
            });
    }
}