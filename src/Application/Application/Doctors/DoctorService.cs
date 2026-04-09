using Abstractions.Messaging;
using Abstractions.Persistence.Repositories;
using Contracts.Doctors;
using Models.Doctors;

namespace Application.Doctors;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IDoctorEventPublisher _doctorEventPublisher;

    public DoctorService(IDoctorRepository doctorRepository, IDoctorEventPublisher doctorEventPublisher)
    {
        _doctorRepository = doctorRepository;
        _doctorEventPublisher = doctorEventPublisher;
    }

    public async Task<Doctor> AddDoctorAsync(
        string name,
        DoctorSpecialty specialty,
        string license,
        string phone,
        string email,
        CancellationToken cancellationToken)
    {
        return await _doctorRepository.AddAsync(
            new Doctor(
            Id: 0,
            Name: name,
            Specialty: specialty,
            License: license,
            Phone: phone,
            Email: email,
            IsActive: true),
            cancellationToken);
    }

    public async Task<Doctor> GetDoctorAsync(long id, CancellationToken cancellationToken)
    {
        return await _doctorRepository.GetAsync(id, cancellationToken);
    }

    public async Task<Doctor> UpdateDoctorAsync(
        long id,
        string name,
        DoctorSpecialty specialty,
        string license,
        string phone,
        string email,
        bool isActive,
        CancellationToken cancellationToken)
    {
        Doctor doctor = await _doctorRepository.UpdateAsync(
            new Doctor(
                Id: 0,
                Name: name,
                Specialty: specialty,
                License: license,
                Phone: phone,
                Email: email,
                IsActive: isActive),
            cancellationToken);

        if (!isActive)
        {
            await _doctorEventPublisher.PublishFiredAsync(id, DateTime.Now, cancellationToken);
        }

        return doctor;
    }
}