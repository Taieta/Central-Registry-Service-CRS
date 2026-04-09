using Abstractions.Persistence.Repositories;
using Models.Doctors;
using Models.Doctors.Filters;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace Persistence.Repositories.Doctors;

public class DoctorRepository : IDoctorRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DoctorRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO doctor (
                               doctor_name,
                               doctor_specialty,
                               doctor_license,
                               doctor_phone,
                               doctor_email,
                               doctor_is_active
                           )
                           VALUES (
                               @Name,
                               @Specialty,
                               @License,
                               @Phone,
                               @Email,
                               @IsActive
                           )
                           RETURNING doctor_id
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Name", doctor.Name));
        command.Parameters.Add(new NpgsqlParameter("Specialty", doctor.Specialty));
        command.Parameters.Add(new NpgsqlParameter("License", doctor.License));
        command.Parameters.Add(new NpgsqlParameter("Phone", doctor.Phone));
        command.Parameters.Add(new NpgsqlParameter("Email", doctor.Email));
        command.Parameters.Add(new NpgsqlParameter("IsActive", doctor.IsActive));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        await reader.ReadAsync(cancellationToken);

        return doctor with
        {
            Id = reader.GetInt64("doctor_id"),
        };
    }

    public async Task<Doctor> GetAsync(long id, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               doctor_id,
                               doctor_name,
                               doctor_specialty,
                               doctor_license,
                               doctor_phone,
                               doctor_email,
                               doctor_is_active
                           FROM doctor
                           WHERE doctor_id = @Id
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("Id", id));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException($"Doctor with id {id} not found");
        }

        return new Doctor(
            Id: reader.GetInt64("doctor_id"),
            Name: reader.GetString("doctor_name"),
            Specialty: reader.GetFieldValue<DoctorSpecialty>("doctor_specialty"),
            License: reader.GetString("doctor_license"),
            Phone: reader.GetString("doctor_phone"),
            Email: reader.GetString("doctor_email"),
            IsActive: reader.GetBoolean("doctor_is_active"));
    }

    public async Task<Doctor> UpdateAsync(Doctor doctor, CancellationToken cancellationToken)
    {
        const string sql = """
                           UPDATE doctor
                           SET
                               doctor_name = @Name,
                               doctor_specialty = @Specialty,
                               doctor_license = @License,
                               doctor_phone = @Phone,
                               doctor_email = @Email,
                               doctor_is_active = @IsActive
                           WHERE doctor_id = @Id
                           RETURNING
                               id,
                               name,
                               specialty,
                               license,
                               phone,
                               email,
                               is_active
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Id", doctor.Id));
        command.Parameters.Add(new NpgsqlParameter("Name", doctor.Name));
        command.Parameters.Add(new NpgsqlParameter("Specialty", doctor.Specialty));
        command.Parameters.Add(new NpgsqlParameter("License", doctor.License));
        command.Parameters.Add(new NpgsqlParameter("Phone", doctor.Phone));
        command.Parameters.Add(new NpgsqlParameter("Email", doctor.Email));
        command.Parameters.Add(new NpgsqlParameter("IsActive", doctor.IsActive));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException($"Doctor with id {doctor.Id} not found");
        }

        return new Doctor(
            Id: reader.GetInt64("doctor_id"),
            Name: reader.GetString("doctor_name"),
            Specialty: reader.GetFieldValue<DoctorSpecialty>("doctor_specialty"),
            License: reader.GetString("doctor_license"),
            Phone: reader.GetString("doctor_phone"),
            Email: reader.GetString("doctor_email"),
            IsActive: reader.GetBoolean("doctor_is_active"));
    }

    public async IAsyncEnumerable<Doctor> SearchAsync(
        DoctorFilter filter,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               doctor_id,
                               doctor_name,
                               doctor_specialty,
                               doctor_license,
                               doctor_phone,
                               doctor_email,
                               doctor_is_active
                           FROM doctor
                           WHERE
                               (doctor_id > @Cursor)
                               AND (@Name IS NULL OR doctor_name ILIKE @Name)
                               AND (doctor_specialty = @Specialty OR @Specialty IS NULL)
                           ORDER BY doctor_id
                           LIMIT @PageSize
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Cursor", filter.Cursor));
        command.Parameters.Add(new NpgsqlParameter("PageSize", filter.PageSize));

        command.Parameters.Add(
            new NpgsqlParameter(
                "Name",
                filter.Name is null ? DBNull.Value : $"%{filter.Name}%"));

        command.Parameters.Add(
            new NpgsqlParameter("Specialty", filter.Specialty.HasValue ? filter.Specialty.Value : DBNull.Value));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Doctor(
                Id: reader.GetInt64("doctor_id"),
                Name: reader.GetString("doctor_name"),
                Specialty: reader.GetFieldValue<DoctorSpecialty>("doctor_specialty"),
                License: reader.GetString("doctor_license"),
                Phone: reader.GetString("doctor_phone"),
                Email: reader.GetString("doctor_email"),
                IsActive: reader.GetBoolean("doctor_is_active"));
        }
    }
}