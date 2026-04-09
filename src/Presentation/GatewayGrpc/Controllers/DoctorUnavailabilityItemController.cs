using CentralRegistry.Grpc;
using Contracts.DoctorSchedules;
using Grpc.Core;

namespace GatewayGrpc.Controllers;

public class DoctorUnavailabilityItemController : DoctorUnavailabilityService.DoctorUnavailabilityServiceBase
{
    private readonly IDoctorScheduleService _doctorScheduleService;

    public DoctorUnavailabilityItemController(IDoctorScheduleService doctorScheduleService)
    {
        _doctorScheduleService = doctorScheduleService;
    }

    public override async Task<CreateUnavailabilityItemResponse> CreateUnavailabilityItem(
        CreateUnavailabilityItemRequest request,
        ServerCallContext context)
    {
        await _doctorScheduleService.CreateUnavailabilityItemAsync(
            doctorId: request.DoctorId,
            startTime: request.StartTime.ToDateTime(),
            endTime: request.EndTime.ToDateTime(),
            reason: request.Reason,
            cancellationToken: context.CancellationToken);

        return new CreateUnavailabilityItemResponse();
    }
}