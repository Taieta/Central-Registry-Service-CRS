using Abstractions.Messaging;
using Abstractions.Persistence.Repositories;
using Contracts.Patients;
using Models.Patients;

namespace Application.Patients;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IPatientEventPublisher _patientEventPublisher;

    public PatientService(IPatientRepository patientRepository, IPatientEventPublisher patientEventPublisher)
    {
        _patientRepository = patientRepository;
        _patientEventPublisher = patientEventPublisher;
    }

    public async Task<Patient> AddPatientAsync(
        string name,
        bool isMale,
        string phone,
        string email,
        DateTime birthDate,
        BloodType bloodType,
        string insurance,
        CancellationToken cancellationToken)
    {
        Patient patient = await _patientRepository.AddAsync(
            new Patient(
                Id: 0,
                Name: name,
                IsMale: isMale,
                Phone: phone,
                Email: email,
                BirthDate: birthDate,
                BloodType: bloodType,
                Insurance: insurance),
            cancellationToken);

        await _patientEventPublisher.PublishCreatedAsync(patient, cancellationToken);

        return patient;
    }

    public async Task<Patient> GetPatientAsync(long id, CancellationToken cancellationToken)
    {
        return await _patientRepository.GetAsync(id, cancellationToken);
    }

    public async Task<Patient> UpdatePatientAsync(
        long id,
        string name,
        bool isMale,
        string phone,
        string email,
        DateTime birthDate,
        BloodType bloodType,
        string insurance,
        CancellationToken cancellationToken)
    {
        return await _patientRepository.UpdateAsync(
            new Patient(
                Id: id,
                Name: name,
                IsMale: isMale,
                Phone: phone,
                Email: email,
                BirthDate: birthDate,
                BloodType: bloodType,
                Insurance: insurance),
            cancellationToken);
    }
}