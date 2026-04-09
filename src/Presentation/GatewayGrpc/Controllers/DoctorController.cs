using CentralRegistry.Grpc;
using Contracts.Doctors;
using GatewayGrpc.Mappers;
using Grpc.Core;
using Models.Doctors;

namespace GatewayGrpc.Controllers;

public class DoctorController : DoctorService.DoctorServiceBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    public override async Task<CreateDoctorResponse> CreateDoctor(
        CreateDoctorRequest request,
        ServerCallContext context)
    {
        Doctor doctor = await _doctorService.AddDoctorAsync(
            request.Name,
            request.Specialty.FromDoctorSpecialtyModel(),
            request.License,
            request.Phone,
            request.Email,
            context.CancellationToken);

        var response = new CreateDoctorResponse
        {
            Doctor = doctor.ToDoctorModel(),
        };

        return response;
    }

    public override async Task<GetDoctorByIdResponse> GetDoctorById(GetDoctorByIdRequest request, ServerCallContext context)
    {
        Doctor doctor = await _doctorService.GetDoctorAsync(
            request.Id,
            context.CancellationToken);

        var response = new GetDoctorByIdResponse
        {
            Doctor = doctor.ToDoctorModel(),
        };

        return response;
    }

    public override async Task<UpdateDoctorResponse> UpdateDoctor(
        UpdateDoctorRequest request,
        ServerCallContext context)
    {
        Doctor doctor = await _doctorService.UpdateDoctorAsync(
            request.Id,
            request.Name,
            request.Specialty.FromDoctorSpecialtyModel(),
            request.License,
            request.Phone,
            request.Email,
            request.IsActive,
            context.CancellationToken);

        var response = new UpdateDoctorResponse
        {
            Doctor = doctor.ToDoctorModel(),
        };

        return response;
    }

    public override async Task<DeleteDoctorResponse> DeleteDoctor(
        DeleteDoctorRequest request,
        ServerCallContext context)
    {
        await _doctorService.UpdateDoctorAsync(
            request.Id,
            name: string.Empty,
            specialty: DoctorSpecialty.Therapist,
            license: string.Empty,
            phone: string.Empty,
            email: string.Empty,
            isActive: false,
            context.CancellationToken);

        return new DeleteDoctorResponse();
    }
}