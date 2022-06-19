using System;
using System.Windows;
using System.Windows.Controls;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.ViewModels;
using BlackSmith.Presentation.Views.Pages;

namespace BlackSmith.Presentation;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Dashboard _dashboardPage;
    private readonly DoctorList _doctorListPage;
    private readonly DoctorListViewModel _doctorListViewModel;
    private readonly PatientCreate _patientCreatePage;
    private readonly PatientCreateViewModel _patientCreateViewModel;
    private readonly PatientEdit _patientEditPage;
    private readonly PatientEditViewModel _patientEditViewModel;
    private readonly PatientList _patientListPage;
    private readonly PatientListViewModel _patientListViewModel;
    private readonly ScheduleList _scheduleListPage;
    private readonly Settings _settingsPage;

    public MainWindow(
        INavService navService,
        Dashboard dashboardPage,
        PatientListViewModel patientListViewModel,
        PatientList patientListPage,
        PatientCreateViewModel patientCreateViewModel,
        PatientCreate patientCreatePage,
        PatientEdit patientEditPage,
        PatientEditViewModel patientEditViewModel,
        DoctorList doctorListPage,
        DoctorListViewModel doctorListViewModel,
        ScheduleList scheduleListPage,
        Settings settingsPage
    )
    {
        _dashboardPage = dashboardPage;
        _patientListPage = patientListPage;
        _patientListViewModel = patientListViewModel;
        _patientCreatePage = patientCreatePage;
        _patientEditPage = patientEditPage;
        _patientEditViewModel = patientEditViewModel;
        _doctorListPage = doctorListPage;
        _doctorListViewModel = doctorListViewModel;
        _scheduleListPage = scheduleListPage;
        _settingsPage = settingsPage;
        _patientCreateViewModel = patientCreateViewModel;

        navService.NavigationTriggered += OnNavigationTriggered;
        InitializeComponent();
    }

    private void OnNavigationTriggered(object? sender, NavigationTriggeredEventArgs e)
    {
        GoToPage(e.Page, e.Model);
    }

    private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
    {
        Enum.TryParse<Pages>(sender.Current.Page.Name, out var pageEnum);
        GoToPage(pageEnum);
    }

    private void GoToPage(Pages page, object? model = null)
    {
        Page pageToGo;
        switch (page)
        {
            case Pages.PatientCreate:
                _patientCreatePage.DataContext = _patientCreateViewModel;
                pageToGo = _patientCreatePage;
                break;
            case Pages.PatientList:
                _patientListPage.DataContext = _patientListViewModel;
                pageToGo = _patientListPage;
                break;
            case Pages.PatientEdit:
                _patientEditPage.DataContext = _patientEditViewModel;
                if (model is not null) _patientEditViewModel.Patient = (Patient)model;
                pageToGo = _patientEditPage;
                break;
            case Pages.Home:
                pageToGo = _dashboardPage;
                break;
            case Pages.DoctorList:
                _doctorListPage.DataContext = _doctorListViewModel;
                pageToGo = _doctorListPage;
                break;
            case Pages.ScheduleList:
                pageToGo = _scheduleListPage;
                break;
            case Pages.Settings:
                pageToGo = _settingsPage;
                break;
            default:
                pageToGo = null;
                break;
        }

        RootFrame.NavigationService.Navigate(pageToGo);
    }

    private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        // We check what theme is currently
        // active and choose its opposite.
        var newTheme = Theme.GetAppTheme() == ThemeType.Dark
            ? ThemeType.Light
            : ThemeType.Dark;

        // We apply the theme to the entire application.
        Theme.Apply(newTheme);
    }
}