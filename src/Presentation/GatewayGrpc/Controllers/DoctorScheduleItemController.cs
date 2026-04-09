using CentralRegistry.Grpc;
using Contracts.DoctorSchedules;
using GatewayGrpc.Mappers;
using Grpc.Core;
using Models.DoctorSchedules;

namespace GatewayGrpc.Controllers;

public class DoctorScheduleItemController : DoctorScheduleService.DoctorScheduleServiceBase
{
    private readonly IDoctorScheduleService _doctorScheduleService;

    public DoctorScheduleItemController(IDoctorScheduleService doctorScheduleService)
    {
        _doctorScheduleService = doctorScheduleService;
    }

    public override async Task<CreateScheduleItemResponse> CreateScheduleItem(
        CreateScheduleItemRequest request,
        ServerCallContext context)
    {
        DoctorScheduleItem item =
            await _doctorScheduleService.CreateScheduleItemAsync(
                doctorId: request.DoctorId,
                weekday: request.Weekday.FromWeekdayModel(),
                startTime: request.StartTime.ToDateTime(),
                endTime: request.EndTime.ToDateTime(),
                cancellationToken: context.CancellationToken);

        return new CreateScheduleItemResponse
        {
            Item = item.ToDoctorScheduleItemModel(),
        };
    }

    public override async Task<GetScheduleItemByIdResponse> GetScheduleItemById(
        GetScheduleItemByIdRequest request,
        ServerCallContext context)
    {
        DoctorScheduleItem item =
            await _doctorScheduleService.GetScheduleItemAsync(
                request.Id,
                context.CancellationToken);

        return new GetScheduleItemByIdResponse
        {
            Item = item.ToDoctorScheduleItemModel(),
        };
    }
}