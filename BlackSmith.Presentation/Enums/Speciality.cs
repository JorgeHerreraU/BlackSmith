using System.ComponentModel;

namespace BlackSmith.Presentation.Enums;

public enum Speciality
{
    Allergology,
    Anesthesiology,
    Cardiology,
    Dermatology,
    Endocrinology,
    Gastroenterology,

    [Description("General Practice")]
    GeneralPractice,
    Hematology,

    [Description("Infectious Diseases")]
    InfectiousDiseases,
    Nephrology,
    Neurology,
    Oncology,
    Ophthalmology,
    Orthopedics,
    Osteopathy,
    Pathology,
    Pediatrics,

    [Description("Physical Medicine")]
    PhysicalMedicine,
    Psychiatry,
    Radiology,
    Rheumatology,
    Surgery,
    Urology
}
