using CentralRegistry.Grpc;
using Google.Protobuf.WellKnownTypes;
using Models.DoctorSchedules;

namespace GatewayGrpc.Mappers;

public static class DoctorScheduleItemModelMapper
{
    public static DoctorScheduleItemModel ToDoctorScheduleItemModel(this DoctorScheduleItem scheduleItem)
    {
        return new DoctorScheduleItemModel
        {
            Id = scheduleItem.Id,
            DoctorId = scheduleItem.DoctorId,
            Weekday = scheduleItem.Weekday.ToWeekdayModel(),
            StartTime = scheduleItem.StartTime.ToTimestamp(),
            EndTime = scheduleItem.EndTime.ToTimestamp(),
        };
    }

    public static WeekdayModel ToWeekdayModel(this Weekday weekday)
    {
        return weekday switch
        {
            Weekday.Monday => WeekdayModel.Monday,
            Weekday.Tuesday => WeekdayModel.Tuesday,
            Weekday.Wednesday => WeekdayModel.Wednesday,
            Weekday.Thursday => WeekdayModel.Thursday,
            Weekday.Friday => WeekdayModel.Friday,
            Weekday.Saturday => WeekdayModel.Saturday,
            Weekday.Sunday => WeekdayModel.Sunday,
            _ => WeekdayModel.WeekdayUnspecified,
        };
    }

    public static Weekday FromWeekdayModel(this WeekdayModel weekdayModel)
    {
        return weekdayModel switch
        {
            WeekdayModel.Monday => Weekday.Monday,
            WeekdayModel.Tuesday => Weekday.Tuesday,
            WeekdayModel.Wednesday => Weekday.Wednesday,
            WeekdayModel.Thursday => Weekday.Thursday,
            WeekdayModel.Friday => Weekday.Friday,
            WeekdayModel.Saturday => Weekday.Saturday,
            WeekdayModel.Sunday => Weekday.Sunday,
            WeekdayModel.WeekdayUnspecified => throw new ArgumentOutOfRangeException(nameof(weekdayModel), $"Unsupported weekday value: {weekdayModel}"),
            _ => throw new ArgumentOutOfRangeException(nameof(weekdayModel), $"Unsupported weekday value: {weekdayModel}"),
        };
    }
}