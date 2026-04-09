namespace Models.Patients;

public record Patient(long Id, string Name, bool IsMale, string Phone, string Email, DateTime BirthDate, BloodType BloodType, string Insurance);