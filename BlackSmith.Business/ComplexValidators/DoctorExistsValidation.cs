using BlackSmith.Business.Interfaces;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;

namespace BlackSmith.Business.ComplexValidators;

public class DoctorExistsValidation : ICreateComplexValidation<Doctor>
{
    private readonly IRepository<Doctor> _repository;
    public DoctorExistsValidation(IRepository<Doctor> repository)
    {
        _repository = repository;
    }
    public async Task ValidateAsync(Doctor doctor)
    {
        if (await DoctorEmailExists(doctor.Email))
            throw new ArgumentException("Doctor e-mail address is already registered");
    }

    private async Task<bool> DoctorEmailExists(string email)
    {
        return await _repository.Get(d => string.Equals(d.Email, email)) is not null;
    }
}
