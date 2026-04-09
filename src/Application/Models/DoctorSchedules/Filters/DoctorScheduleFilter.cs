using Models.Doctors;

namespace Models.DoctorSchedules.Filters;

public record DoctorScheduleFilter(
    long[] DoctorIds,
    DoctorSpecialty? Specialty,
    Weekday? Weekday);