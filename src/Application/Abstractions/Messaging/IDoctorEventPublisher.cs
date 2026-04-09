namespace Abstractions.Messaging;

public interface IDoctorEventPublisher
{
    Task PublishUnavailableAsync(
        long doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default);

    Task PublishFiredAsync(
        long doctorId,
        DateTime firedDate,
        CancellationToken ct = default);
}