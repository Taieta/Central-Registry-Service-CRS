using Models.Patients;

namespace Abstractions.Messaging;

public interface IPatientEventPublisher
{
    Task PublishCreatedAsync(
        Patient patient,
        CancellationToken ct = default);
}