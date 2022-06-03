using System;
using System.Windows;
using BlackSmith.Core.IoC;
using BlackSmith.Data;
using BlackSmith.Presentation.Controls;
using BlackSmith.Presentation.Modules.Appointments;
using BlackSmith.Presentation.Modules.Home;
using BlackSmith.Presentation.Modules.Patients;
using BlackSmith.Presentation.Store;
using Microsoft.Extensions.DependencyInjection;

namespace BlackSmith.Presentation;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App()
    {
        SeedData.Initialize();
        var servicesCollection = new ServiceCollection();
        DependencyInjection.RegisterSharedDependencies(ref servicesCollection);
        RegisterLocalDependencies(ref servicesCollection);
        _services = servicesCollection.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _services.GetRequiredService<MainWindow>();

        mainWindow.DataContext = _services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    private static void RegisterLocalDependencies(ref ServiceCollection services)
    {
        services.AddScoped<NavigationStore>();
        services.AddScoped<MainWindowViewModel>();

        services.AddSingleton<MainWindow>();
        services.AddSingleton<NavbarViewModel>();
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<AppointmentViewModel>();
        services.AddSingleton<AppointmentCreateViewModel>();
        services.AddSingleton<AppointmentListViewModel>();
        services.AddSingleton<PatientViewModel>();
        services.AddSingleton<PatientCreateViewModel>();
        services.AddSingleton<PatientListViewModel>();
    }
}