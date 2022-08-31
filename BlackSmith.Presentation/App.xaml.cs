using AutoMapper;
using BlackSmith.Data;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.ViewModels;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service;
using BlackSmith.Service.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace BlackSmith.Presentation;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private readonly IServiceProvider _services;

    public App()
    {
        SeedData.Initialize();

        var servicesCollection = new ServiceCollection();
        var mapperConfig = new MapperConfigurationExpression();
        ServiceDependencies.RegisterDependencies(ref servicesCollection);
        ServiceDependencies.RegisterAutoMapperConfiguration(ref mapperConfig);
        RegisterLocalDependencies(ref servicesCollection);
        RegisterLocalAutoMapperConfiguration(ref mapperConfig);
        var config = new MapperConfiguration(mapperConfig);
        servicesCollection.AddSingleton(config.CreateMapper());
        _services = servicesCollection.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _services.GetRequiredService<MainWindow>();

        mainWindow.Show();

        mainWindow.DataContext = _services.GetRequiredService<MainWindowViewModel>();

        base.OnStartup(e);
    }

    private static void RegisterLocalDependencies(ref ServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IModalService, ModalService>();
        services.AddSingleton<Settings>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IEventAggregator, EventAggregator>();

        services.AddSingleton<MainWindow>();
        services.AddScoped<MainWindowViewModel>();

        services.AddSingleton<Home>();
        services.AddSingleton<HomeViewModel>();

        // Patient Pages
        services.AddSingleton<PatientCreate>();
        services.AddSingleton<PatientCreateViewModel>();
        services.AddSingleton<PatientList>();
        services.AddSingleton<PatientListViewModel>();
        services.AddSingleton<PatientEdit>();
        services.AddSingleton<PatientEditViewModel>();

        // Doctor Pages
        services.AddSingleton<DoctorList>();
        services.AddSingleton<DoctorListViewModel>();
        services.AddSingleton<DoctorDetail>();
        services.AddSingleton<DoctorDetailViewModel>();
        services.AddSingleton<DoctorCreate>();
        services.AddSingleton<DoctorCreateViewModel>();
        services.AddSingleton<DoctorEdit>();
        services.AddSingleton<DoctorEditViewModel>();

        // Schedule Pages
        services.AddSingleton<ScheduleList>();
        services.AddSingleton<ScheduleListViewModel>();
        services.AddSingleton<ScheduleCreate>();
        services.AddSingleton<ScheduleCreateViewModel>();
    }

    private static void RegisterLocalAutoMapperConfiguration(ref MapperConfigurationExpression mapperConfig)
    {
        mapperConfig.CreateMap<Patient, PatientDTO>().ReverseMap();
        mapperConfig.CreateMap<Address, AddressDTO>().ReverseMap();
        mapperConfig.CreateMap<Doctor, DoctorDTO>().ReverseMap();
        mapperConfig.CreateMap<WorkingDay, WorkingDayDTO>().ReverseMap();
        mapperConfig.CreateMap<Appointment, AppointmentDTO>().ReverseMap();
    }
}