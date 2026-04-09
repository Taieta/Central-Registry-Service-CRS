using Abstractions.Messaging;
using Itmo.Dev.Platform.Kafka.Extensions;
using Mds.Kafka.Events.Contracts;
using MdsKafka.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MdsKafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMdsKafka(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Presentation:Kafka"))
            .AddProducer(producer => producer
                .WithKey<PatientCreationKey>()
                .WithValue<PatientCreationValue>()
                .WithConfiguration(configuration.GetSection("Presentation:Kafka:Producers:PatientCreation"))
                .SerializeKeyWithProto()
                .SerializeValueWithProto()));

        collection.AddScoped<IPatientEventPublisher, PatientEventPublisher>();

        return collection;
    }
}
