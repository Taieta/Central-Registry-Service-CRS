using Abstractions.Persistence.Repositories;
using Models.DoctorSchedules;
using Models.DoctorSchedules.Filters;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace Persistence.Repositories.DoctorUnavailabilityItems;

public class DoctorUnavailabilityItemRepository : IDoctorUnavailabilityItemRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DoctorUnavailabilityItemRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<DoctorUnavailabilityItem> AddAsync(
        DoctorUnavailabilityItem unavailabilityItem,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO doctor_unavailability (
                               doctor_id,
                               doctor_unavailability_start_time,
                               doctor_unavailability_end_time,
                               doctor_unavailability_reason
                           )
                           VALUES (
                               @DoctorId,
                               @StartTime,
                               @EndTime,
                               @Reason
                           )
                           RETURNING doctor_unavailability_id
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("DoctorId", unavailabilityItem.DoctorId));
        command.Parameters.Add(new NpgsqlParameter("StartTime", unavailabilityItem.StartTime));
        command.Parameters.Add(new NpgsqlParameter("EndTime", unavailabilityItem.EndTime));
        command.Parameters.Add(new NpgsqlParameter("Reason", unavailabilityItem.Reason));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        await reader.ReadAsync(cancellationToken);

        return unavailabilityItem with
        {
            Id = reader.GetInt64("doctor_unavailability_id"),
        };
    }

    public async Task<DoctorUnavailabilityItem> GetAsync(
        long id,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               doctor_schedule_id,
                               doctor_id,
                               doctor_unavailability_start_time,
                               doctor_unavailability_end_time,
                               doctor_unavailability_reason
                           FROM doctor_unavailability
                           WHERE doctor_schedule_id = @Id
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
                $"DoctorScheduleItem with id {id} not found");
        }

        return new DoctorUnavailabilityItem(
            Id: reader.GetInt64("doctor_schedule_id"),
            DoctorId: reader.GetInt64("doctor_id"),
            StartTime: reader.GetDateTime("doctor_unavailability_start_time"),
            EndTime: reader.GetDateTime("doctor_unavailability_end_time"),
            Reason: reader.GetString("doctor_unavailability_reason"));
    }

    public async IAsyncEnumerable<DoctorUnavailabilityItem> SearchAsync(
        DoctorScheduleFilter filter,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               u.doctor_unavailability_id,
                               u.doctor_id,
                               u.doctor_unavailability_start_time,
                               u.doctor_unavailability_end_time,
                               u.doctor_unavailability_reason
                           FROM doctor_unavailability u
                           JOIN doctor d
                               ON d.doctor_id = u.doctor_id
                           WHERE
                               (cardinality(@DoctorIds) = 0 OR u.doctor_id = ANY(@DoctorIds))
                               AND (d.doctor_specialty = @Specialty OR @Specialty IS NULL)
                               AND (d.doctor_is_active = TRUE)
                           ORDER BY u.doctor_unavailability_id
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("DoctorIds", filter.DoctorIds));
        command.Parameters.Add(new NpgsqlParameter("Specialty", filter.Specialty.HasValue ? filter.Specialty.Value : DBNull.Value));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new DoctorUnavailabilityItem(
                Id: reader.GetInt64("doctor_unavailability_id"),
                DoctorId: reader.GetInt64("doctor_id"),
                StartTime: reader.GetDateTime("doctor_unavailability_start_time"),
                EndTime: reader.GetDateTime("doctor_unavailability_end_time"),
                Reason: reader.GetString("doctor_unavailability_reason"));
        }
    }
}