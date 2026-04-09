using AmsGrpc.Mappers;
using Contracts.DoctorSchedules;
using Doctor.Schedule;
using Grpc.Core;
using Models.DoctorSchedules;

namespace AmsGrpc.Controllers;

public class DoctorScheduleController : DoctorScheduleService.DoctorScheduleServiceBase
{
    private readonly IDoctorScheduleService _doctorScheduleService;

    public DoctorScheduleController(IDoctorScheduleService doctorScheduleService)
    {
        _doctorScheduleService = doctorScheduleService;
    }

    public override async Task<SearchScheduleBySpecializationResponse> SearchScheduleBySpecialization(
        SearchScheduleBySpecializationRequest request,
        ServerCallContext context)
    {
        var response = new SearchScheduleBySpecializationResponse();

        await foreach (DoctorScheduleItem item in _doctorScheduleService
            .SearchScheduleBySpecializationAsync(
                request.Specialization.FromDoctorSpecialtyModel(),
                request.Weekday.FromWeekdayModel(),
                context.CancellationToken))
        {
            response.ScheduleItems.Add(item.ToScheduleItem());
        }

        return response;
    }

    public override async Task<SearchUnavailabilityBySpecializationResponse> SearchUnavailabilityBySpecialization(
        SearchUnavailabilityBySpecializationRequest request,
        ServerCallContext context)
    {
        var response = new SearchUnavailabilityBySpecializationResponse();

        await foreach (DoctorUnavailabilityItem item in _doctorScheduleService
            .SearchUnavailabilityBySpecializationAsync(
                request.Specialization.FromDoctorSpecialtyModel(),
                context.CancellationToken))
        {
            response.UnavailabilityItems.Add(item.ToUnavailabilityItem());
        }

        return response;
    }

    public override async Task<SearchScheduleByIdResponse> SearchScheduleById(
        SearchScheduleByIdRequest request,
        ServerCallContext context)
    {
        var response = new SearchScheduleByIdResponse();

        await foreach (DoctorScheduleItem item in _doctorScheduleService
            .SearchScheduleByIdAsync(
                request.DoctorId,
                context.CancellationToken))
        {
            response.ScheduleItems.Add(item.ToScheduleItem());
        }

        return response;
    }

    public override async Task<SearchUnavailabilityByIdResponse> SearchUnavailabilityById(
        SearchUnavailabilityByIdRequest request,
        ServerCallContext context)
    {
        var response = new SearchUnavailabilityByIdResponse();

        await foreach (DoctorUnavailabilityItem item in _doctorScheduleService
            .SearchUnavailabilityByIdAsync(
                request.DoctorId,
                context.CancellationToken))
        {
            response.UnavailabilityItems.Add(item.ToUnavailabilityItem());
        }

        return response;
    }
}