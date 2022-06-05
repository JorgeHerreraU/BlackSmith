﻿using AutoMapper;
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

namespace BlackSmith.Core.IoC;

public static class DependencyInjection
{
    public static void RegisterSharedDependencies(ref ServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<AppDbContextFactory>();
        services.AddSingleton<AppointmentsBL>();
        services.AddSingleton<PatientsBL>();
        services.AddSingleton<IAppointmentService, AppointmentService>();
        services.AddSingleton<IPatientService, PatientService>();
        services.AddScoped<IValidator<Patient>, PatientValidator>();
    }

    public static void RegisterSharedAutoMapperConfiguration(ref MapperConfigurationExpression mapperConfig)
    {
        mapperConfig.AddProfile(new AppointmentsProfile());
        mapperConfig.AddProfile(new DoctorsProfile());
        mapperConfig.AddProfile(new PatientsProfile());
        mapperConfig.AddProfile(new AddressesProfile());
    }
}