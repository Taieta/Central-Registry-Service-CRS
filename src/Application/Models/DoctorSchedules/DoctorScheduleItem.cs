namespace Models.DoctorSchedules;

public record DoctorScheduleItem(long Id, long DoctorId, Weekday Weekday, DateTime StartTime, DateTime EndTime);