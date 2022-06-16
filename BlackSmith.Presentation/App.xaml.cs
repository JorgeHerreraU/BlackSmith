using System;
using System.Windows;
using AutoMapper;
using BlackSmith.Core.IoC;
using BlackSmith.Data;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.ViewModels;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using Microsoft.Extensions.DependencyInjection;

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
        DependencyInjection.RegisterSharedDependencies(ref servicesCollection);
        DependencyInjection.RegisterSharedAutoMapperConfiguration(ref mapperConfig);
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
        services.AddScoped<MainWindowViewModel>();
        services.AddSingleton<INavService, NavService>();
        services.AddSingleton<IMessageService, MessageService>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<Dashboard>();
        services.AddSingleton<PatientCreate>();
        services.AddSingleton<PatientCreateViewModel>();
        services.AddSingleton<PatientList>();
        services.AddSingleton<PatientListViewModel>();
        services.AddSingleton<PatientEdit>();
        services.AddSingleton<PatientEditViewModel>();
    }

    private static void RegisterLocalAutoMapperConfiguration(ref MapperConfigurationExpression mapperConfig)
    {
        mapperConfig.CreateMap<Patient, PatientDTO>();
        mapperConfig.CreateMap<PatientDTO, Patient>();
        mapperConfig.CreateMap<Address, AddressDTO>();
        mapperConfig.CreateMap<AddressDTO, Address>();
    }
}