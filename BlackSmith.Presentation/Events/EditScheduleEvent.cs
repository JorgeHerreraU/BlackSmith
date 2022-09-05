using BlackSmith.Presentation.Models;
using Prism.Events;

namespace BlackSmith.Presentation.Events;

public class EditScheduleEvent : PubSubEvent<Appointment> { }
