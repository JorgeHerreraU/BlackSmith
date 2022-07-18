using BlackSmith.Presentation.Models;
using Prism.Events;

namespace BlackSmith.Presentation.Events;

public class EditPatientEvent : PubSubEvent<Patient>
{
}