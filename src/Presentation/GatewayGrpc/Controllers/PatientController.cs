using CentralRegistry.Grpc;
using Contracts.Patients;
using GatewayGrpc.Mappers;
using Grpc.Core;
using Models.Patients;

namespace GatewayGrpc.Controllers;

public class PatientController : PatientService.PatientServiceBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    public override async Task<CreatePatientResponse> CreatePatient(
        CreatePatientRequest request,
        ServerCallContext context)
    {
        Patient patient = await _patientService.AddPatientAsync(
            request.Name,
            request.IsMale,
            request.Phone,
            request.Email,
            request.BirthDate.ToDateTime(),
            request.BloodType.FromBloodTypeModel(),
            request.Insurance,
            context.CancellationToken);

        var response = new CreatePatientResponse
        {
            Patient = patient.ToPatientModel(),
        };

        return response;
    }

    public override async Task<GetPatientByIdResponse> GetPatientById(GetPatientByIdRequest request, ServerCallContext context)
    {
        Patient patient = await _patientService.GetPatientAsync(
            request.Id,
            context.CancellationToken);

        var response = new GetPatientByIdResponse
        {
            Patient = patient.ToPatientModel(),
        };

        return response;
    }

    public override async Task<UpdatePatientResponse> UpdatePatient(
        UpdatePatientRequest request,
        ServerCallContext context)
    {
        Patient patient = await _patientService.UpdatePatientAsync(
            request.Id,
            request.Name,
            request.IsMale,
            request.Phone,
            request.Email,
            request.BirthDate.ToDateTime(),
            request.BloodType.FromBloodTypeModel(),
            request.Insurance,
            context.CancellationToken);

        var response = new UpdatePatientResponse
        {
            Patient = patient.ToPatientModel(),
        };

        return response;
    }
}