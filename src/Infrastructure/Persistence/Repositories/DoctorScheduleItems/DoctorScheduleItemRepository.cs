using Abstractions.Persistence.Repositories;
using Models.DoctorSchedules;
using Models.DoctorSchedules.Filters;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace Persistence.Repositories.DoctorScheduleItems;

public class DoctorScheduleItemRepository : IDoctorScheduleItemRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DoctorScheduleItemRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<DoctorScheduleItem> AddAsync(
        DoctorScheduleItem doctorScheduleItem,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO doctor_schedule (
                               doctor_id,
                               doctor_schedule_weekday,
                               doctor_schedule_start_time,
                               doctor_schedule_end_time
                           )
                           VALUES (
                               @DoctorId,
                               @Weekday,
                               @StartTime,
                               @EndTime
                           )
                           RETURNING doctor_schedule_id
                           """;

        await using NpgsqlConnection connection =
            await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("DoctorId", doctorScheduleItem.DoctorId));
        command.Parameters.Add(new NpgsqlParameter("Weekday", doctorScheduleItem.Weekday));
        command.Parameters.Add(new NpgsqlParameter("StartTime", doctorScheduleItem.StartTime));
        command.Parameters.Add(new NpgsqlParameter("EndTime", doctorScheduleItem.EndTime));

        await using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync(cancellationToken);

        await reader.ReadAsync(cancellationToken);

        return doctorScheduleItem with
        {
            Id = reader.GetInt64("doctor_schedule_id"),
        };
    }

    public async Task<DoctorScheduleItem> GetAsync(
        long id,
        CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               doctor_schedule_id,
                               doctor_id,
                               doctor_schedule_weekday,
                               doctor_schedule_start_time,
                               doctor_schedule_end_time
                           FROM doctor_schedule
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

        return new DoctorScheduleItem(
            Id: reader.GetInt64("doctor_schedule_id"),
            DoctorId: reader.GetInt64("doctor_id"),
            Weekday: reader.GetFieldValue<Weekday>("doctor_schedule_weekday"),
            StartTime: reader.GetDateTime("doctor_schedule_start_time"),
            EndTime: reader.GetDateTime("doctor_schedule_end_time"));
    }

    public async IAsyncEnumerable<DoctorScheduleItem> SearchAsync(
        DoctorScheduleFilter filter,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT
                               s.doctor_schedule_id,
                               s.doctor_id,
                               s.doctor_schedule_weekday,
                               s.doctor_schedule_start_time,
                               s.doctor_schedule_end_time
                           FROM doctor_schedule s
                           JOIN doctor d
                               ON d.doctor_id = s.doctor_id
                           WHERE
                               (cardinality(@DoctorIds) = 0 OR s.doctor_id = ANY(@DoctorIds))
                               AND (d.doctor_specialty = @Specialty OR @Specialty IS NULL)
                               AND (s.doctor_schedule_weekday = @Weekday OR @Weekday IS NULL)
                               AND (d.doctor_is_active = TRUE)
                           ORDER BY s.doctor_schedule_id
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("DoctorIds", filter.DoctorIds));
        command.Parameters.Add(new NpgsqlParameter("Specialty", filter.Specialty.HasValue ? filter.Specialty.Value : DBNull.Value));
        command.Parameters.Add(new NpgsqlParameter("Weekday", filter.Weekday.HasValue ? filter.Weekday.Value : DBNull.Value));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new DoctorScheduleItem(
                Id: reader.GetInt64("doctor_schedule_id"),
                DoctorId: reader.GetInt64("doctor_id"),
                Weekday: reader.GetFieldValue<Weekday>("doctor_schedule_weekday"),
                StartTime: reader.GetDateTime("doctor_schedule_start_time"),
                EndTime: reader.GetDateTime("doctor_schedule_end_time"));
        }
    }
}