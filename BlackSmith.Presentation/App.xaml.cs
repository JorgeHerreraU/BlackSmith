using System;
using System.Windows;
using BlackSmith.Data;
using BlackSmith.Data.Repositories;
using BlackSmith.Domain.Repositories;
using BlackSmith.Presentation.Store;
using BlackSmith.Presentation.ViewModels;
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
        _services = RegisterServices();
        // using var scope = _services.CreateScope();
        // var services = scope.ServiceProvider;
        // SeedData.Initialize(services);
        SeedData.Initialize();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _services.GetRequiredService<MainWindow>();

        mainWindow.DataContext = _services.GetRequiredService<MainViewModel>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    private IServiceProvider RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<NavigationStore>();
        services.AddScoped<MainViewModel>();
        services.AddSingleton<AppDbContextFactory>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSingleton<MainWindow>();
        services.AddSingleton<NavbarViewModel>();
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<AppointmentViewModel>();
        services.AddSingleton<AppointmentCreateViewModel>();
        services.AddSingleton<AppointmentIndexViewModel>();

        return services.BuildServiceProvider();
    }
}