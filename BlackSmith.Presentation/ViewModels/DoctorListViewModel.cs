using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using BlackSmith.Presentation.Commands;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Models;
using BlackSmith.Service.Interfaces;
using JetBrains.Annotations;

namespace BlackSmith.Presentation.ViewModels;

public class DoctorListViewModel : BindableBase
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly INavService _navService;
    private IEnumerable<Doctor> _allDoctors = new List<Doctor>();
    private ObservableCollection<Doctor> _doctors = null!;
    private string _searchInput = "";


    public DoctorListViewModel(
        IMapper mapper,
        IMessageService messageService,
        INavService navService,
        IDoctorService doctorService)
    {
        _mapper = mapper;
        _messageService = messageService;
        _navService = navService;
        _doctorService = doctorService;

        LoadData();

        ClearSearchCommand = new RelayCommand(OnClearSearch);
        GoToCreate = new RelayCommand(OnCreate);
        EditCommand = new RelayCommand<Doctor>(OnEdit);
        DeleteCommand = new RelayCommand<Doctor>(OnDelete);
    }

    public ObservableCollection<Doctor> Doctors
    {
        get => _doctors;
        set
        {
            _doctors = value;
            NotifyPropertyChanged();
        }
    }

    public string SearchInput
    {
        get => _searchInput;
        set
        {
            _searchInput = value;
            NotifyPropertyChanged();
            FilterData(_searchInput);
        }
    }

    public RelayCommand GoToCreate { get; }
    public RelayCommand ClearSearchCommand { get; }
    public RelayCommand<Doctor> EditCommand { get; }
    public RelayCommand<Doctor> DeleteCommand { get; }

    private void FilterData(string searchInput)
    {
        var isSearchInputNull = string.IsNullOrWhiteSpace(searchInput);
        var doctors = new ObservableCollection<Doctor>(_allDoctors);
        var filteredResults = new ObservableCollection<Doctor>(
            _allDoctors.ToList().Where(c =>
                c.FullName.ToLower().Contains(searchInput.ToLower())
            ));
        Doctors = isSearchInputNull ? doctors : filteredResults;
    }

    private void OnDelete(Doctor obj)
    {
        throw new NotImplementedException();
    }

    private void OnEdit(Doctor obj)
    {
        throw new NotImplementedException();
    }

    private void OnCreate()
    {
        throw new NotImplementedException();
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