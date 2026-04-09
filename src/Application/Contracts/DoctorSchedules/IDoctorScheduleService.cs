using Models.Doctors;
using Models.DoctorSchedules;

namespace Contracts.DoctorSchedules;

public interface IDoctorScheduleService
{
    Task<DoctorScheduleItem> CreateScheduleItemAsync(long doctorId, Weekday weekday, DateTime startTime, DateTime endTime, CancellationToken cancellationToken);

    Task<DoctorUnavailabilityItem> CreateUnavailabilityItemAsync(long doctorId, DateTime startTime, DateTime endTime, string reason, CancellationToken cancellationToken);

    Task<DoctorScheduleItem> GetScheduleItemAsync(long id, CancellationToken cancellationToken);

    Task<DoctorUnavailabilityItem> GetUnavailabilityItemAsync(long id, CancellationToken cancellationToken);

    IAsyncEnumerable<DoctorScheduleItem> SearchScheduleBySpecializationAsync(DoctorSpecialty specialty, Weekday weekday, CancellationToken cancellationToken);

    IAsyncEnumerable<DoctorUnavailabilityItem> SearchUnavailabilityBySpecializationAsync(DoctorSpecialty specialty, CancellationToken cancellationToken);

    IAsyncEnumerable<DoctorScheduleItem> SearchScheduleByIdAsync(long doctorId, CancellationToken cancellationToken);

    IAsyncEnumerable<DoctorUnavailabilityItem> SearchUnavailabilityByIdAsync(long doctorId, CancellationToken cancellationToken);
}