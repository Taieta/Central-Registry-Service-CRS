using Doctor.Schedule;
using Google.Protobuf.WellKnownTypes;
using Models.DoctorSchedules;
using DoctorSpecialtyModel = Doctor.Schedule.DoctorSpecialty;
using WeekdayModel = Doctor.Schedule.Weekday;

namespace AmsGrpc.Mappers;

public static class DoctorScheduleModelMapper
{
    public static WeekdayModel ToWeekdayModel(this Models.DoctorSchedules.Weekday weekday)
    {
        return weekday switch
        {
            Models.DoctorSchedules.Weekday.Monday => WeekdayModel.Monday,
            Models.DoctorSchedules.Weekday.Tuesday => WeekdayModel.Tuesday,
            Models.DoctorSchedules.Weekday.Wednesday => WeekdayModel.Wednesday,
            Models.DoctorSchedules.Weekday.Thursday => WeekdayModel.Thursday,
            Models.DoctorSchedules.Weekday.Friday => WeekdayModel.Friday,
            Models.DoctorSchedules.Weekday.Saturday => WeekdayModel.Saturday,
            Models.DoctorSchedules.Weekday.Sunday => WeekdayModel.Sunday,
            _ => WeekdayModel.Unspecified,
        };
    }

    public static Models.DoctorSchedules.Weekday FromWeekdayModel(this WeekdayModel weekdayModel)
    {
        return weekdayModel switch
        {
            WeekdayModel.Monday => Models.DoctorSchedules.Weekday.Monday,
            WeekdayModel.Tuesday => Models.DoctorSchedules.Weekday.Tuesday,
            WeekdayModel.Wednesday => Models.DoctorSchedules.Weekday.Wednesday,
            WeekdayModel.Thursday => Models.DoctorSchedules.Weekday.Thursday,
            WeekdayModel.Friday => Models.DoctorSchedules.Weekday.Friday,
            WeekdayModel.Saturday => Models.DoctorSchedules.Weekday.Saturday,
            WeekdayModel.Sunday => Models.DoctorSchedules.Weekday.Sunday,
            WeekdayModel.Unspecified => throw new ArgumentOutOfRangeException(nameof(weekdayModel), $"Unsupported weekday value: {weekdayModel}"),
            _ => throw new ArgumentOutOfRangeException(nameof(weekdayModel), $"Unsupported weekday value: {weekdayModel}"),
        };
    }

    public static DoctorSpecialtyModel ToDoctorSpecialtyModel(this Models.Doctors.DoctorSpecialty specialty)
    {
        return specialty switch
        {
            Models.Doctors.DoctorSpecialty.Cardiologist => DoctorSpecialtyModel.Cardiologist,
            Models.Doctors.DoctorSpecialty.Surgeon => DoctorSpecialtyModel.Surgeon,
            Models.Doctors.DoctorSpecialty.Pediatrician => DoctorSpecialtyModel.Pediatrician,
            Models.Doctors.DoctorSpecialty.Therapist => DoctorSpecialtyModel.Therapist,
            _ => DoctorSpecialtyModel.Unspecified,
        };
    }

    public static Models.Doctors.DoctorSpecialty FromDoctorSpecialtyModel(this DoctorSpecialtyModel specialtyModel)
    {
        return specialtyModel switch
        {
            DoctorSpecialtyModel.Cardiologist => Models.Doctors.DoctorSpecialty.Cardiologist,
            DoctorSpecialtyModel.Surgeon => Models.Doctors.DoctorSpecialty.Surgeon,
            DoctorSpecialtyModel.Pediatrician => Models.Doctors.DoctorSpecialty.Pediatrician,
            DoctorSpecialtyModel.Therapist => Models.Doctors.DoctorSpecialty.Therapist,
            DoctorSpecialtyModel.Unspecified => throw new ArgumentOutOfRangeException(nameof(specialtyModel), $"Unsupported specialty value: {specialtyModel}"),
            _ => throw new ArgumentOutOfRangeException(nameof(specialtyModel), $"Unsupported specialty value: {specialtyModel}"),
        };
    }

    public static ScheduleItem ToScheduleItem(this DoctorScheduleItem item)
    {
        return new ScheduleItem
        {
            DoctorId = item.DoctorId,
            StartTime = DateTime.SpecifyKind(item.StartTime, DateTimeKind.Utc).ToTimestamp(),
            EndTime = DateTime.SpecifyKind(item.EndTime, DateTimeKind.Utc).ToTimestamp(),
            Weekday = item.Weekday.ToWeekdayModel(),
        };
    }

    public static UnavailabilityItem ToUnavailabilityItem(this DoctorUnavailabilityItem item)
    {
        return new UnavailabilityItem
        {
            DoctorId = item.DoctorId,
            StartDate = DateTime.SpecifyKind(item.StartTime, DateTimeKind.Utc).ToTimestamp(),
            EndDate = DateTime.SpecifyKind(item.EndTime, DateTimeKind.Utc).ToTimestamp(),
        };
    }
}