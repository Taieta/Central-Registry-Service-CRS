using AmsGrpc.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AmsGrpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IEndpointRouteBuilder MapAmsGrpc(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<DoctorScheduleController>();
        return endpoints;
    }
}