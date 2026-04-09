using Models.Patients;

namespace Abstractions.Persistence.Repositories;

public interface IPatientRepository
{
    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken);

    Task<Patient> GetAsync(long id, CancellationToken cancellationToken);

    Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken);
}