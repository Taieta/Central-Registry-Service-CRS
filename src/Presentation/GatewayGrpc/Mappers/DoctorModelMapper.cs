using CentralRegistry.Grpc;
using Models.Doctors;

namespace GatewayGrpc.Mappers;

public static class DoctorModelMapper
{
    public static DoctorModel ToDoctorModel(this Doctor doctor)
    {
        return new DoctorModel
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialty = doctor.Specialty.ToDoctorSpecialtyModel(),
            License = doctor.License,
            Phone = doctor.Phone,
            Email = doctor.Email,
            IsActive = doctor.IsActive,
        };
    }

    public static DoctorSpecialtyModel ToDoctorSpecialtyModel(this DoctorSpecialty specialty)
    {
        return specialty switch
        {
            DoctorSpecialty.Cardiologist => DoctorSpecialtyModel.DoctorSpecialtyCardiologist,
            DoctorSpecialty.Surgeon => DoctorSpecialtyModel.DoctorSpecialtySurgeon,
            DoctorSpecialty.Pediatrician => DoctorSpecialtyModel.DoctorSpecialtyPediatrician,
            DoctorSpecialty.Therapist => DoctorSpecialtyModel.DoctorSpecialtyTherapist,
            _ => DoctorSpecialtyModel.DoctorSpecialtyUnspecified,
        };
    }

    public static DoctorSpecialty FromDoctorSpecialtyModel(this DoctorSpecialtyModel specialty)
    {
        return specialty switch
        {
            DoctorSpecialtyModel.DoctorSpecialtyCardiologist => DoctorSpecialty.Cardiologist,
            DoctorSpecialtyModel.DoctorSpecialtySurgeon => DoctorSpecialty.Surgeon,
            DoctorSpecialtyModel.DoctorSpecialtyPediatrician => DoctorSpecialty.Pediatrician,
            DoctorSpecialtyModel.DoctorSpecialtyTherapist => DoctorSpecialty.Therapist,
            DoctorSpecialtyModel.DoctorSpecialtyUnspecified => throw new ArgumentOutOfRangeException(nameof(specialty), $"Unsupported specialty value: {specialty}"),
            _ => throw new ArgumentOutOfRangeException(nameof(specialty), $"Unsupported specialty value: {specialty}"),
        };
    }
}