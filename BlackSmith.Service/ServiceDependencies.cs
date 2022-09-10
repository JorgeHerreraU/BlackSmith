using AutoMapper;
using BlackSmith.Business.Components;
using BlackSmith.Business.Validators;
using BlackSmith.Data;
using BlackSmith.Data.Repositories;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using BlackSmith.Service.Interfaces;
using BlackSmith.Service.Profiles;
using BlackSmith.Service.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BlackSmith.Service;

public static class ServiceDependencies
{
    public static void RegisterDependencies(ref ServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<AppDbContextFactory>();
        services.AddSingleton<AppointmentsBL>();
        services.AddSingleton<PatientsBL>();
        services.AddSingleton<DoctorsBL>();
        services.AddSingleton<AppointmentsDoctorsBL>();
        services.AddSingleton<IAppointmentService, AppointmentService>();
        services.AddSingleton<IPatientService, PatientService>();
        services.AddSingleton<IDoctorService, DoctorService>();
        services.AddScoped<IValidator<Patient>, PatientValidator>();
        services.AddScoped<IValidator<Doctor>, DoctorValidator>();
        services.AddScoped<IValidator<Appointment>, AppointmentValidator>();
    }

    public static void RegisterAutoMapperConfiguration(
        ref MapperConfigurationExpression mapperConfig
    )
    {
        mapperConfig.AddProfile(new AppointmentsProfile());
        mapperConfig.AddProfile(new DoctorsProfile());
        mapperConfig.AddProfile(new PatientsProfile());
        mapperConfig.AddProfile(new AddressesProfile());
    }
}
