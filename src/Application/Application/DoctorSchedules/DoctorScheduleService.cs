using Abstractions.Messaging;
using Abstractions.Persistence.Repositories;
using Contracts.DoctorSchedules;
using Models.Doctors;
using Models.DoctorSchedules;
using Models.DoctorSchedules.Filters;

namespace Application.DoctorSchedules;

public class DoctorScheduleService : IDoctorScheduleService
{
    private readonly IDoctorScheduleItemRepository _doctorScheduleItemRepository;
    private readonly IDoctorUnavailabilityItemRepository _doctorUnavailabilityItemRepository;
    private readonly IDoctorEventPublisher _doctorEventPublisher;

    public DoctorScheduleService(IDoctorScheduleItemRepository doctorScheduleItemRepository, IDoctorUnavailabilityItemRepository doctorUnavailabilityItemRepository, IDoctorEventPublisher doctorEventPublisher)
    {
        _doctorScheduleItemRepository = doctorScheduleItemRepository;
        _doctorUnavailabilityItemRepository = doctorUnavailabilityItemRepository;
        _doctorEventPublisher = doctorEventPublisher;
    }

    public async Task<DoctorScheduleItem> CreateScheduleItemAsync(
        long doctorId,
        Weekday weekday,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken)
    {
        return await _doctorScheduleItemRepository.AddAsync(
            new DoctorScheduleItem(
                0,
                doctorId,
                weekday,
                startTime,
                endTime),
            cancellationToken);
    }

    public async Task<DoctorUnavailabilityItem> CreateUnavailabilityItemAsync(
        long doctorId,
        DateTime startTime,
        DateTime endTime,
        string reason,
        CancellationToken cancellationToken)
    {
        DoctorUnavailabilityItem unavailabilityItem = await _doctorUnavailabilityItemRepository.AddAsync(
            new DoctorUnavailabilityItem(
                0,
                doctorId,
                startTime,
                endTime,
                reason),
            cancellationToken);

        await _doctorEventPublisher.PublishUnavailableAsync(doctorId, startTime,  endTime, cancellationToken);

        return unavailabilityItem;
    }

    public async Task<DoctorScheduleItem> GetScheduleItemAsync(long id, CancellationToken cancellationToken)
    {
        return await _doctorScheduleItemRepository.GetAsync(id, cancellationToken);
    }

    public async Task<DoctorUnavailabilityItem> GetUnavailabilityItemAsync(long id, CancellationToken cancellationToken)
    {
        return await _doctorUnavailabilityItemRepository.GetAsync(id, cancellationToken);
    }

    public IAsyncEnumerable<DoctorScheduleItem> SearchScheduleBySpecializationAsync(
        DoctorSpecialty specialty,
        Weekday weekday,
        CancellationToken cancellationToken)
    {
        return _doctorScheduleItemRepository.SearchAsync(
            new DoctorScheduleFilter(
                [],
                Specialty: specialty,
                Weekday: weekday),
            cancellationToken);
    }

    public IAsyncEnumerable<DoctorUnavailabilityItem> SearchUnavailabilityBySpecializationAsync(
        DoctorSpecialty specialty,
        CancellationToken cancellationToken)
    {
        return _doctorUnavailabilityItemRepository.SearchAsync(
            new DoctorScheduleFilter(
                [],
                Specialty: specialty,
                Weekday: null),
            cancellationToken);
    }

    public IAsyncEnumerable<DoctorScheduleItem> SearchScheduleByIdAsync(long doctorId, CancellationToken cancellationToken)
    {
        return _doctorScheduleItemRepository.SearchAsync(
            new DoctorScheduleFilter(
                DoctorIds: [doctorId],
                Specialty: null,
                Weekday: null),
            cancellationToken);
    }

    public IAsyncEnumerable<DoctorUnavailabilityItem> SearchUnavailabilityByIdAsync(long doctorId, CancellationToken cancellationToken)
    {
        return _doctorUnavailabilityItemRepository.SearchAsync(
            new DoctorScheduleFilter(
                DoctorIds: [doctorId],
                Specialty: null,
                Weekday: null),
            cancellationToken);
    }
}