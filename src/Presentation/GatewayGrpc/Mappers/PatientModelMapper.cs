using CentralRegistry.Grpc;
using Google.Protobuf.WellKnownTypes;
using Models.Patients;

namespace GatewayGrpc.Mappers;

public static class PatientModelMapper
{
    public static PatientModel ToPatientModel(this Patient patient)
    {
        return new PatientModel
        {
            Id = patient.Id,
            Name = patient.Name,
            BirthDate = patient.BirthDate.ToTimestamp(),
            IsMale = patient.IsMale,
            BloodType = patient.BloodType.ToBloodTypeModel(),
            Phone = patient.Phone,
            Email = patient.Email,
            Insurance = patient.Insurance,
        };
    }

    public static BloodTypeModel ToBloodTypeModel(this BloodType bloodType)
    {
        return bloodType switch
        {
            BloodType.APos => BloodTypeModel.BloodTypeAPos,
            BloodType.ANeg => BloodTypeModel.BloodTypeANeg,
            BloodType.BPos => BloodTypeModel.BloodTypeAPos,
            BloodType.BNeg => BloodTypeModel.BloodTypeANeg,
            BloodType.AbPos => BloodTypeModel.BloodTypeAbPos,
            BloodType.AbNeg => BloodTypeModel.BloodTypeAbNeg,
            BloodType.OPos => BloodTypeModel.BloodTypeOPos,
            BloodType.ONeg => BloodTypeModel.BloodTypeONeg,
            _ => BloodTypeModel.BloodTypeUnspecified,
        };
    }

    public static BloodType FromBloodTypeModel(this BloodTypeModel bloodTypeModel)
    {
        return bloodTypeModel switch
        {
            BloodTypeModel.BloodTypeAPos => BloodType.APos,
            BloodTypeModel.BloodTypeANeg => BloodType.ANeg,
            BloodTypeModel.BloodTypeBPos => BloodType.BPos,
            BloodTypeModel.BloodTypeBNeg => BloodType.BNeg,
            BloodTypeModel.BloodTypeAbPos => BloodType.AbPos,
            BloodTypeModel.BloodTypeAbNeg => BloodType.AbNeg,
            BloodTypeModel.BloodTypeOPos => BloodType.OPos,
            BloodTypeModel.BloodTypeONeg => BloodType.ONeg,
            BloodTypeModel.BloodTypeUnspecified => throw new ArgumentOutOfRangeException(nameof(bloodTypeModel), bloodTypeModel, "Unsupported blood type model"),
            _ => throw new ArgumentOutOfRangeException(nameof(bloodTypeModel), bloodTypeModel, "Unsupported blood type model"),
        };
    }
}