using AutoMapper;
using BlackSmith.Presentation.Modules.Appointments;
using BlackSmith.Service.DTOs;

namespace BlackSmith.Presentation.Profiles;

public class EditableAppointmentsProfile : Profile
{
    public EditableAppointmentsProfile()
    {
        CreateMap<SimpleEditableAppointment, AppointmentDTO>().ReverseMap();
    }
}