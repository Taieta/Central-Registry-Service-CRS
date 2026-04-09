using Abstractions.Messaging;
using AmsKafka.Publishers;
using Crs.Events.Kafka.Contracts;
using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmsKafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmsKafka(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Presentation:Kafka"))
            .AddProducer(producer => producer
                .WithKey<DoctorEventKey>()
                .WithValue<DoctorEventValue>()
                .WithConfiguration(configuration.GetSection("Presentation:Kafka:Producers:DoctorEvent"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()));

        collection.AddScoped<IDoctorEventPublisher, DoctorEventPublisher>();

        return collection;
    }
}
