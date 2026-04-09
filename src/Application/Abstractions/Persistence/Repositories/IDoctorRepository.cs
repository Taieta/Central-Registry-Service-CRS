using Models.Doctors;
using Models.Doctors.Filters;

namespace Abstractions.Persistence.Repositories;

public interface IDoctorRepository
{
    Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken);

    Task<Doctor> GetAsync(long id, CancellationToken cancellationToken);

    Task<Doctor> UpdateAsync(Doctor doctor, CancellationToken cancellationToken);

    IAsyncEnumerable<Doctor> SearchAsync(DoctorFilter filter, CancellationToken cancellationToken);
}