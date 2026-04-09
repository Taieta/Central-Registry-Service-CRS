using Models.DoctorSchedules;
using Models.DoctorSchedules.Filters;

namespace Abstractions.Persistence.Repositories;

public interface IDoctorUnavailabilityItemRepository
{
    Task<DoctorUnavailabilityItem> AddAsync(DoctorUnavailabilityItem unavailabilityItem, CancellationToken cancellationToken);

    Task<DoctorUnavailabilityItem> GetAsync(long id, CancellationToken cancellationToken);

    IAsyncEnumerable<DoctorUnavailabilityItem> SearchAsync(DoctorScheduleFilter filter, CancellationToken cancellationToken);
}