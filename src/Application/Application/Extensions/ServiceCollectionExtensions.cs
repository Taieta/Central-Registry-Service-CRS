using Application.Doctors;
using Application.DoctorSchedules;
using Application.Patients;
using Contracts.Doctors;
using Contracts.DoctorSchedules;
using Contracts.Patients;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IPatientService, PatientService>();
        collection.AddScoped<IDoctorService, DoctorService>();
        collection.AddScoped<IDoctorScheduleService, DoctorScheduleService>();

        return collection;
    }
}
