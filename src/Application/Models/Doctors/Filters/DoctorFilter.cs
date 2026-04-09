namespace Models.Doctors.Filters;

public record DoctorFilter(string? Name, DoctorSpecialty? Specialty, int Cursor, int PageSize);