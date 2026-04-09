namespace Models.Doctors;

public record Doctor(long Id, string Name, DoctorSpecialty Specialty, string License, string Phone, string Email, bool IsActive);