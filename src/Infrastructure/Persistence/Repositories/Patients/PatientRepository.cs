using Abstractions.Persistence.Repositories;
using Models.Patients;
using Npgsql;
using System.Data;

namespace Persistence.Repositories.Patients;

public class PatientRepository : IPatientRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PatientRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<Patient> AddAsync(
        Patient patient,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO patient (
                               patient_name,
                               patient_is_male,
                               patient_phone,
                               patient_email,
                               patient_birth_date,
                               patient_blood_type,
                               patient_insurance
                           )
                           VALUES (
                               @Name,
                               @IsMale,
                               @Phone,
                               @Email,
                               @BirthDate,
                               @BloodType,
                               @Insurance
                           )
                           RETURNING patient_id
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Name", patient.Name));
        command.Parameters.Add(new NpgsqlParameter("IsMale", patient.IsMale));
        command.Parameters.Add(new NpgsqlParameter("Phone", patient.Phone));
        command.Parameters.Add(new NpgsqlParameter("Email", patient.Email));
        command.Parameters.Add(new NpgsqlParameter("BirthDate", patient.BirthDate));
        command.Parameters.Add(new NpgsqlParameter("BloodType", patient.BloodType));
        command.Parameters.Add(new NpgsqlParameter("Insurance", patient.Insurance));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        await reader.ReadAsync(cancellationToken);

        return patient with
        {
            Id = reader.GetInt64("patient_id"),
        };
    }

    public async Task<Patient> GetAsync(
        long id,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               patient_id,
                               patient_name,
                               patient_is_male,
                               patient_phone,
                               patient_email,
                               patient_birth_date,
                               patient_blood_type,
                               patient_insurance
                           FROM patient
                           WHERE patient_id = @Id
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("Id", id));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException(
                $"Patient with id {id} not found");
        }

        return new Patient(
            Id: reader.GetInt64("patient_id"),
            Name: reader.GetString("patient_name"),
            IsMale: reader.GetBoolean("patient_is_male"),
            Phone: reader.GetString("patient_phone"),
            Email: reader.GetString("patient_email"),
            BirthDate: reader.GetDateTime("patient_birth_date"),
            BloodType: reader.GetFieldValue<BloodType>("patient_blood_type"),
            Insurance: reader.GetString("patient_insurance"));
    }

    public async Task<Patient> UpdateAsync(
    Patient patient,
    CancellationToken cancellationToken)
    {
        const string sql = """
        UPDATE patient
        SET
            patient_name = @Name,
            patient_is_male = @IsMale,
            patient_phone = @Phone,
            patient_email = @Email,
            patient_birth_date = @BirthDate,
            patient_blood_type = @BloodType,
            patient_insurance = @Insurance
        WHERE patient_id = @Id
        RETURNING
            patient_id,
            patient_name,
            patient_is_male,
            patient_phone,
            patient_email,
            patient_birth_date,
            patient_blood_type,
            patient_insurance
        """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Id", patient.Id));
        command.Parameters.Add(new NpgsqlParameter("Name", patient.Name));
        command.Parameters.Add(new NpgsqlParameter("IsMale", patient.IsMale));
        command.Parameters.Add(new NpgsqlParameter("Phone", patient.Phone));
        command.Parameters.Add(new NpgsqlParameter("Email", patient.Email));
        command.Parameters.Add(new NpgsqlParameter("BirthDate", patient.BirthDate));
        command.Parameters.Add(new NpgsqlParameter("BloodType", patient.BloodType));
        command.Parameters.Add(new NpgsqlParameter("Insurance", patient.Insurance));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException(
                $"Patient with id {patient.Id} not found");
        }

        return new Patient(
            Id: reader.GetInt64("patient_id"),
            Name: reader.GetString("patient_name"),
            IsMale: reader.GetBoolean("patient_is_male"),
            Phone: reader.GetString("patient_phone"),
            Email: reader.GetString("patient_email"),
            BirthDate: reader.GetDateTime("patient_birth_date"),
            BloodType: reader.GetFieldValue<BloodType>("patient_blood_type"),
            Insurance: reader.GetString("patient_insurance"));
    }
}