using Abstractions.Messaging;
using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Mds.Kafka.Events.Contracts;
using Models.Patients;

namespace MdsKafka.Publishers;

public class PatientEventPublisher : IPatientEventPublisher
{
    private readonly IKafkaMessageProducer<PatientCreationKey, PatientCreationValue> _producer;

    public PatientEventPublisher(
        IKafkaMessageProducer<PatientCreationKey, PatientCreationValue> producer)
    {
        _producer = producer;
    }

    public async Task PublishCreatedAsync(
        Patient patient,
        CancellationToken ct = default)
    {
        var key = new PatientCreationKey
        {
            PatientId = patient.Id,
        };

        var value = new PatientCreationValue
        {
            PatientCreated = new PatientCreationValue.Types.PatientCreated
            {
                PatientId = patient.Id,
                CreationTime = DateTime.UtcNow.ToTimestamp(),
            },
        };

        var message = new KafkaProducerMessage<PatientCreationKey, PatientCreationValue>(key, value);

        await _producer.ProduceAsync(message, ct);
    }
}