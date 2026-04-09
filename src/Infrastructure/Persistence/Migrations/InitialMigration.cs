using FluentMigrator;

namespace Persistence.Migrations;

[Migration(1, "InitialMigration_AllModels")]
public class InitialMigration : Migration
{
    public override void Up()
    {
        Execute.Sql("""
        -- Enums
        CREATE TYPE doctor_specialty AS ENUM ('Cardiologist', 'Surgeon', 'Pediatrician', 'Therapist');
        CREATE TYPE blood_type AS ENUM ('APos', 'ANeg', 'BPos', 'BNeg', 'AbPos', 'AbNeg', 'OPos', 'ONeg');
        CREATE TYPE weekday AS ENUM ('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday');

        -- Doctor table
        CREATE TABLE doctor
        (
            doctor_id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
            doctor_name TEXT NOT NULL,
            doctor_specialty doctor_specialty NOT NULL,
            doctor_license TEXT NOT NULL,
            doctor_phone TEXT NOT NULL,
            doctor_email TEXT NOT NULL,
            doctor_is_active BOOLEAN NOT NULL
        );

        -- Patient table
        CREATE TABLE patient
        (
            patient_id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
            patient_name TEXT NOT NULL,
            patient_is_male BOOLEAN NOT NULL,
            patient_phone TEXT NOT NULL,
            patient_email TEXT NOT NULL,
            patient_birth_date TIMESTAMP NOT NULL,
            patient_blood_type blood_type NOT NULL,
            patient_insurance TEXT NOT NULL
        );

        -- DoctorScheduleItem table
        CREATE TABLE doctor_schedule
        (
            doctor_schedule_id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
            doctor_id BIGINT NOT NULL,
            doctor_schedule_weekday weekday NOT NULL,
            doctor_schedule_start_time TIMESTAMP NOT NULL,
            doctor_schedule_end_time TIMESTAMP NOT NULL
        );

        -- DoctorUnavailabilityItem table
        CREATE TABLE doctor_unavailability
        (
            doctor_unavailability_id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
            doctor_id BIGINT NOT NULL,
            doctor_unavailability_start_time TIMESTAMP NOT NULL,
            doctor_unavailability_end_time TIMESTAMP NOT NULL,
            doctor_unavailability_reason TEXT NOT NULL
        );
        """);
    }

    public override void Down()
    {
        Execute.Sql("""
        DROP TABLE IF EXISTS doctor_unavailability;
        DROP TABLE IF EXISTS doctor_schedule;
        DROP TABLE IF EXISTS patient;
        DROP TABLE IF EXISTS doctor;

        DROP TYPE IF EXISTS weekday;
        DROP TYPE IF EXISTS blood_type;
        DROP TYPE IF EXISTS doctor_specialty;
        """);
    }
}
