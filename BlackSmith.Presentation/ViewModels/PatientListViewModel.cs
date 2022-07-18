﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;
using Prism.Events;
using Wpf.Ui.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class PatientListViewModel : ViewModelBase
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavService _navService;
    private readonly IPatientService _patientService;
    private IEnumerable<Patient> _allPatients = new List<Patient>();
    private ObservableCollection<Patient> _patients = null!;
    private string _searchInput = "";

    public PatientListViewModel(INavService navService,
        IModalService modalService,
        IPatientService patientService,
        IMapper mapper,
        IEventAggregator eventAggregator)
    {
        _navService = navService;
        _modalService = modalService;
        _patientService = patientService;
        _mapper = mapper;
        _eventAggregator = eventAggregator;

        LoadPatients();

        ClearSearchCommand = new RelayCommand(OnClearSearch);
        GoToCreate = new RelayCommand(OnCreate);
        EditPatientCommand = new RelayCommand<Patient>(OnEdit);
        DeleteCommand = new RelayCommand<Patient>(OnDelete);
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            _searchInput = value;
            SetValue(value);
            FilterPatients(_searchInput);
        }
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set
        {
            _patients = value;
            SetValue(value);
        }
    }

    public RelayCommand GoToCreate { get; }
    public RelayCommand ClearSearchCommand { get; }
    public RelayCommand<Patient> EditPatientCommand { get; }
    public RelayCommand<Patient> DeleteCommand { get; }

    private async void OnDelete(Patient patient)
    {
        var confirmDeletion = await _modalService.ShowConfirmDialog("Are you sure you want to delete this patient?");
        if (!confirmDeletion) return;
        await _patientService.DeletePatient(_mapper.Map<PatientDTO>(patient));
        LoadPatients();
    }

    private void OnEdit(Patient patient)
    {
        _eventAggregator.GetEvent<EditPatientEvent>().Publish(patient);
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(PatientEdit) });
    }

    private void OnCreate()
    {
        OnClearSearch();
        _navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(PatientCreate) });
    }

    [PublicAPI]
    public async void LoadPatients()
    {
        var patients = await _patientService.GetPatients();
        _allPatients = _mapper.Map<IEnumerable<Patient>>(patients);
        Patients = new ObservableCollection<Patient>(_allPatients);
    }

    private void FilterPatients(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var patients = new ObservableCollection<Patient>(_allPatients);
        var filteredResults = new ObservableCollection<Patient>(_allPatients.ToList()
            .Where(c => c.FullName.ToLower().Contains(searchInput.ToLower())));

        Patients = isSearchInputNull ? patients : filteredResults;
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }
}