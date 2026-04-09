using GatewayGrpc.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace GatewayGrpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IEndpointRouteBuilder MapGatewayGrpc(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<PatientController>();
        endpoints.MapGrpcService<DoctorController>();
        endpoints.MapGrpcService<DoctorScheduleItemController>();
        endpoints.MapGrpcService<DoctorUnavailabilityItemController>();
        return endpoints;
    }
}