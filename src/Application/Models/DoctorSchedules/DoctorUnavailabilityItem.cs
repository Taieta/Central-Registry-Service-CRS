namespace Models.DoctorSchedules;

public record DoctorUnavailabilityItem(long Id, long DoctorId, DateTime StartTime, DateTime EndTime, string Reason);