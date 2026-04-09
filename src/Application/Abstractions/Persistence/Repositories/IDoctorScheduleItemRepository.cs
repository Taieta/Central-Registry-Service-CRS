using Models.DoctorSchedules;
using Models.DoctorSchedules.Filters;

namespace Abstractions.Persistence.Repositories;

public interface IDoctorScheduleItemRepository
{
    Task<DoctorScheduleItem> AddAsync(DoctorScheduleItem doctorScheduleItem, CancellationToken cancellationToken);

    Task<DoctorScheduleItem> GetAsync(long id, CancellationToken cancellationToken);

    IAsyncEnumerable<DoctorScheduleItem> SearchAsync(DoctorScheduleFilter filter, CancellationToken cancellationToken);
}