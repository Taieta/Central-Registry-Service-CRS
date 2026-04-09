using Abstractions.Messaging;
using Crs.Events.Kafka.Contracts;
using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;

namespace AmsKafka.Publishers;

public class DoctorEventPublisher : IDoctorEventPublisher
{
    private readonly IKafkaMessageProducer<DoctorEventKey, DoctorEventValue> _producer;

    public DoctorEventPublisher(
        IKafkaMessageProducer<DoctorEventKey, DoctorEventValue> producer)
    {
        _producer = producer;
    }

    public async Task PublishUnavailableAsync(
        long doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default)
    {
        var key = new DoctorEventKey
        {
            DoctorId = doctorId,
        };

        var value = new DoctorEventValue
        {
            DoctorUnavailable = new DoctorEventValue.Types.DoctorUnavailable
            {
                DoctorId = doctorId,
                StartDate = startDate.ToTimestamp(),
                EndDate = endDate.ToTimestamp(),
            },
        };

        var message =
            new KafkaProducerMessage<DoctorEventKey, DoctorEventValue>(key, value);

        await _producer.ProduceAsync(message, ct);
    }

    public async Task PublishFiredAsync(
        long doctorId,
        DateTime firedDate,
        CancellationToken ct = default)
    {
        var key = new DoctorEventKey
        {
            DoctorId = doctorId,
        };

        var value = new DoctorEventValue
        {
            DoctorFired = new DoctorEventValue.Types.DoctorFired
            {
                DoctorId = doctorId,
                FiredDate = firedDate.ToTimestamp(),
            },
        };

        var message =
            new KafkaProducerMessage<DoctorEventKey, DoctorEventValue>(key, value);

        await _producer.ProduceAsync(message, ct);
    }
}