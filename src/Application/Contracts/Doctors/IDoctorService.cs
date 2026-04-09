using Models.Doctors;

namespace Contracts.Doctors;

public interface IDoctorService
{
    Task<Doctor> AddDoctorAsync(string name, DoctorSpecialty specialty, string license, string phone, string email, CancellationToken cancellationToken);

    Task<Doctor> GetDoctorAsync(long id, CancellationToken cancellationToken);

    Task<Doctor> UpdateDoctorAsync(long id, string name, DoctorSpecialty specialty, string license, string phone, string email, bool isActive, CancellationToken cancellationToken);
}