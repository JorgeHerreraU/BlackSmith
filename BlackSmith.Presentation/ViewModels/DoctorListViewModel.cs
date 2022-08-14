using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Events;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Presentation.Views.Pages;
using BlackSmith.Service.DTOs;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;
using Prism.Events;
using Prism.Mvvm;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorListViewModel : BindableBase
{
    private readonly IDoctorService _doctorService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IMapper _mapper;
    private readonly IModalService _modalService;
    private readonly INavigationService _navigationService;
    private IEnumerable<Doctor> _allDoctors = new List<Doctor>();
    private ObservableCollection<Doctor> _doctors = null!;
    private string _searchInput = "";


    public DoctorListViewModel(IMapper mapper,
        IModalService modalService,
        IDoctorService doctorService,
        IEventAggregator eventAggregator,
        INavigationService navigationService)
    {
        _mapper = mapper;
        _modalService = modalService;
        _doctorService = doctorService;
        _eventAggregator = eventAggregator;
        _navigationService = navigationService;

        LoadData();

        ClearSearchCommand = new RelayCommand(OnClearSearch);
        GoToCreate = new RelayCommand(OnCreate);
        EditCommand = new RelayCommand<Doctor>(OnEdit);
        DeleteCommand = new RelayCommand<Doctor>(OnDelete);
        DetailsCommand = new RelayCommand<Doctor>(OnDetails);
    }

    public ObservableCollection<Doctor> Doctors
    {
        get => _doctors;
        private set
        {
            _doctors = value;
            RaisePropertyChanged();
        }
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            _searchInput = value;
            RaisePropertyChanged();
            FilterData(_searchInput);
        }
    }

    public RelayCommand GoToCreate { get; }
    public RelayCommand ClearSearchCommand { get; }
    public RelayCommand<Doctor> EditCommand { get; }
    public RelayCommand<Doctor> DeleteCommand { get; }
    public RelayCommand<Doctor> DetailsCommand { get; }

    private void OnDetails(Doctor doctor)
    {
        _eventAggregator.GetEvent<DetailsDoctorEvent>().Publish(doctor);
        _navigationService.Navigate(typeof(DoctorDetail));
    }

    private void FilterData(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var doctors = new ObservableCollection<Doctor>(_allDoctors);
        var filteredResults = new ObservableCollection<Doctor>(
            _allDoctors.ToList().Where(c => c.FullName.ToLower().Contains(searchInput.ToLower())));
        Doctors = isSearchInputNull ? doctors : filteredResults;
    }

    private async void OnDelete(Doctor doctor)
    {
        var confirmDeletion = await _modalService.ShowConfirmDialog("Are you sure you want to delete this doctor?");
        if (!confirmDeletion) return;
        await _doctorService.DeleteDoctor(_mapper.Map<DoctorDTO>(doctor));
        LoadData();
    }

    private void OnEdit(Doctor doctor)
    {
        _eventAggregator.GetEvent<EditDoctorEvent>().Publish(doctor);
        _navigationService.Navigate(typeof(DoctorEdit));
    }

    private void OnCreate()
    {
        _eventAggregator.GetEvent<CreateDoctorEvent>().Publish();
        _navigationService.Navigate(typeof(DoctorCreate));
    }

    private void OnClearSearch()
    {
        SearchInput = "";
    }

    [PublicAPI]
    public async void LoadData()
    {
        var doctors = await _doctorService.GetDoctors();
        _allDoctors = _mapper.Map<IEnumerable<Doctor>>(doctors);
        Doctors = new ObservableCollection<Doctor>(_allDoctors);
    }
}