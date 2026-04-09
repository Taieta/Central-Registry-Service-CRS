using Abstractions.Persistence.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Doctors;
using Models.DoctorSchedules;
using Models.Patients;
using Npgsql;
using Npgsql.NameTranslation;
using Persistence.Migrations;
using Persistence.Repositories.Doctors;
using Persistence.Repositories.DoctorScheduleItems;
using Persistence.Repositories.DoctorUnavailabilityItems;
using Persistence.Repositories.Patients;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Postgres")
                                  ?? throw new InvalidOperationException("ConnectionStrings:Postgres is required");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<DoctorSpecialty>("doctor_specialty", nameTranslator: new NpgsqlNullNameTranslator());
        dataSourceBuilder.MapEnum<Weekday>("weekday", nameTranslator: new NpgsqlNullNameTranslator());
        dataSourceBuilder.MapEnum<BloodType>("blood_type", nameTranslator: new NpgsqlNullNameTranslator());
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();
        collection.AddSingleton(dataSource);
        collection.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(InitialMigration).Assembly)
                .For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        collection.AddScoped<IPatientRepository, PatientRepository>();
        collection.AddScoped<IDoctorRepository, DoctorRepository>();
        collection.AddScoped<IDoctorScheduleItemRepository, DoctorScheduleItemRepository>();
        collection.AddScoped<IDoctorUnavailabilityItemRepository, DoctorUnavailabilityItemRepository>();

        return collection;
    }

    public static IServiceProvider RunMigrations(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();

        return serviceProvider;
    }
}