using AutoMapper;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Service.Interfaces;
using Prism.Mvvm;

namespace BlackSmith.Presentation.ViewModels;

public class ScheduleCreateViewModel : BindableBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;
    private readonly INavService _navService;

    public ScheduleCreateViewModel(INavService navService,
        IAppointmentService appointmentService,
        IMapper mapper)
    {
        _navService = navService;
        _appointmentService = appointmentService;
        _mapper = mapper;
    }
}