﻿using System;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Modules.Appointments;
using BlackSmith.Presentation.Modules.Home;
using BlackSmith.Presentation.Modules.Patients;
using BlackSmith.Presentation.Store;

namespace BlackSmith.Presentation.Controls;

public class NavbarViewModel : BindableBase
{
    private readonly AppointmentViewModel _appointmentViewModel;
    private readonly HomeViewModel _homeViewModel;
    private readonly NavigationStore _navigationStore;
    private readonly PatientViewModel _patientViewModel;

    public NavbarViewModel(
        NavigationStore navigationStore,
        HomeViewModel homeViewModel,
        AppointmentViewModel appointmentViewModel,
        PatientViewModel patientViewModel
    )
    {
        _navigationStore = navigationStore;
        _homeViewModel = homeViewModel;
        _appointmentViewModel = appointmentViewModel;
        _patientViewModel = patientViewModel;

        GoToHome = new RelayCommand(OpenHomeView);
        GoToAppointments = new RelayCommand(OpenAppointmentsView);
        GoToPatients = new RelayCommand(OpenPatientsView);
        Quit = new RelayCommand(ExitApplication);
    }

    public RelayCommand GoToHome { get; }
    public RelayCommand GoToAppointments { get; }
    public RelayCommand GoToPatients { get; }
    public RelayCommand Quit { get; }

    private void OpenPatientsView()
    {
        _navigationStore.SelectedViewModel = _patientViewModel;
    }

    private void OpenAppointmentsView()
    {
        _navigationStore.SelectedViewModel = _appointmentViewModel;
    }

    private void OpenHomeView()
    {
        _navigationStore.SelectedViewModel = _homeViewModel;
    }

    private void ExitApplication()
    {
        Environment.Exit(0);
    }
}