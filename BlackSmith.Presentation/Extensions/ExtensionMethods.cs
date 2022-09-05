using BlackSmith.Presentation.Enums;
using BlackSmith.Service.DTOs;
using System;

namespace BlackSmith.Presentation.Extensions;

public static class ExtensionMethods
{
    public static SpecialityDTO ToSpecialityDTO(this Speciality speciality)
    {
        return (SpecialityDTO)Enum.Parse(typeof(SpecialityDTO), speciality.ToString());
    }
}
