using Models.Patients;

namespace Contracts.Patients;

public interface IPatientService
{
    Task<Patient> AddPatientAsync(string name, bool isMale, string phone, string email, DateTime birthDate, BloodType bloodType, string insurance, CancellationToken cancellationToken);

    Task<Patient> GetPatientAsync(long id, CancellationToken cancellationToken);

    Task<Patient> UpdatePatientAsync(long id, string name, bool isMale, string phone, string email, DateTime birthDate, BloodType bloodType, string insurance, CancellationToken cancellationToken);
}