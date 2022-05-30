using BlackSmith.Data;
using BlackSmith.Data.Repositories;
using BlackSmith.Presentation.State.Navigators;
using BlackSmith.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using BlackSmith.Domain.Repositories;
using BlackSmith.Presentation.ViewModels.Factories;

namespace BlackSmith.Presentation;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App()
    {
        _services = RegisterServices();
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

        services.AddSingleton<AppDbContextFactory>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<INavigator, Navigator>();
        services.AddScoped<MainViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddSingleton<IViewModelAbstractFactory, ViewModelAbstractFactory>();
        services.AddSingleton<IViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
        services.AddSingleton<IViewModelFactory<AppointmentViewModel>, AppointmentViewModelFactory>();

        return services.BuildServiceProvider();
    }
}
