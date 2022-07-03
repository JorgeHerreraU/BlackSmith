using System.ComponentModel.DataAnnotations;

namespace BlackSmith.Presentation.Enums;

public enum Speciality
{
    Allergology,
    Anesthesiology,
    Cardiology,
    Dermatology,
    Endocrinology,
    Gastroenterology,
    [Display(Name = "General Practice")] GeneralPractice,
    Hematology,

    [Display(Name = "Infectious Diseases")]
    InfectiousDiseases,
    Nephrology,
    Neurology,
    Oncology,
    Ophthalmology,
    Orthopedics,
    Osteopathy,
    Pathology,
    Pediatrics,
    [Display(Name = "Physical Medicine")] PhysicalMedicine,
    Psychiatry,
    Radiology,
    Rheumatology,
    Surgery,
    Urology
}