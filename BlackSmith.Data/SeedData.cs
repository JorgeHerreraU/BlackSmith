using BlackSmith.Domain.Models;

namespace BlackSmith.Data;

public static class SeedData
{
    public static void Initialize()
    {
        using var context = new AppDbContextFactory().CreateDbContext();

        if (context.Patients.Any())
            return;

        var doctors = new List<Doctor>
        {
            new()
            {
                FirstName = "Constantin",
                LastName = "Horder",
                Email = "chorder0@google.es",
                Phone = "1-(836)810-5892",
                Age = 39,
                Address = new Address
                {
                    Street = "Boulevard",
                    Number = 100,
                    City = "Adelaide",
                    State = "Northwind",
                    Country = "United States",
                    ZipCode = 392982
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Tuesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    }
                }
            },
            new()
            {
                FirstName = "Vincenty",
                LastName = "Baily",
                Email = "vbaily1@phoca.cz",
                Phone = "1-(911)171-5164",
                Age = 53,
                Address = new Address
                {
                    Street = "Rohan",
                    Number = 300,
                    City = "Sydney",
                    State = "New South Wales",
                    Country = "United States",
                    ZipCode = 392331
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Friday
                    }
                }
            },
            new()
            {
                FirstName = "Abramo",
                LastName = "Mathie",
                Email = "amathie2@1und1.de",
                Phone = "1-(464)760-8596",
                Age = 32,
                Address = new Address
                {
                    Street = "Clemont Rd",
                    Number = 32,
                    City = "Arkansas",
                    State = "California",
                    Country = "United States",
                    ZipCode = 654982
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Tuesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    }
                },
                Speciality = Speciality.Cardiology
            },
            new()
            {
                FirstName = "Enrichetta",
                LastName = "Edmands",
                Email = "eedmands3@aol.com",
                Phone = "1-(568)799-1572",
                Age = 40,
                Address = new Address
                {
                    Street = "Douvan Ln",
                    Number = 130,
                    City = "San Francisco",
                    State = "California",
                    Country = "United States",
                    ZipCode = 542382
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Friday
                    }
                }
            },
            new()
            {
                FirstName = "Aili",
                LastName = "Bason",
                Email = "abason4@vk.com",
                Phone = "1-(140)440-7142",
                Age = 24,
                Address = new Address
                {
                    Street = "Garden Ln",
                    Number = 300,
                    City = "Raleigh",
                    State = "Louisiana",
                    Country = "United States",
                    ZipCode = 343282
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Friday
                    }
                }
            },
            new()
            {
                FirstName = "Alexa",
                LastName = "Wassell",
                Email = "awassell5@bigcartel.com",
                Phone = "1-(899)627-4616",
                Age = 60,
                Address = new Address
                {
                    Street = "North Rd",
                    Number = 43,
                    City = "Montgomery",
                    State = "Alabama",
                    Country = "United States",
                    ZipCode = 392982
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Tuesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    }
                },
                Speciality = Speciality.Cardiology
            },
            new()
            {
                FirstName = "Griffie",
                LastName = "Minker",
                Email = "gminker6@privacy.gov.au",
                Phone = "1-(596)317-3539",
                Age = 43,
                Address = new Address
                {
                    Street = "Mondene Rd",
                    Number = 30,
                    City = "Kansas City",
                    State = "Quebec",
                    Country = "United States",
                    ZipCode = 392982
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Friday
                    }
                },
                Speciality = Speciality.Neurology
            },
            new()
            {
                FirstName = "Annabelle",
                LastName = "Stotherfield",
                Email = "astotherfield7@xinhuanet.com",
                Phone = "1-(845)431-7777",
                Age = 33,
                Address = new Address
                {
                    Street = "Slater St",
                    Number = 132,
                    City = "Atlanta",
                    State = "Ohio",
                    Country = "United States",
                    ZipCode = 392982
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Tuesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    }
                },
                Speciality = Speciality.Allergology
            },
            new()
            {
                FirstName = "Evin",
                LastName = "Totaro",
                Email = "etotaro8@wikia.com",
                Phone = "1-(943)210-7540",
                Age = 28,
                Address = new Address
                {
                    Street = "Jouit Rd",
                    Number = 33,
                    City = "Phoenix",
                    State = "British Columbia",
                    Country = "United States",
                    ZipCode = 499382
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Friday
                    }
                }
            },
            new()
            {
                FirstName = "Ari",
                LastName = "Norheny",
                Email = "anorheny9@weather.com",
                Phone = "1-(226)886-0755",
                Age = 31,
                Address = new Address
                {
                    Street = "Lincoln St",
                    Number = 43,
                    City = "Los Angeles",
                    State = "California",
                    Country = "United States",
                    ZipCode = 948382
                },
                WorkingDays = new List<WorkingDay>
                {
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Monday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Tuesday
                    },
                    new()
                    {
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(18, 00),
                        Day = DayOfWeek.Wednesday
                    }
                }
            }
        };

        context.Doctors.AddRange(doctors);
        context.SaveChanges();

        var patients = new List<Patient>
        {
            new()
            {
                FirstName = "Lucas",
                LastName = "San Toro",
                Email = "lucassantoro@email.com",
                Identification = "F38492282",
                Phone = "1-808-874-8432",
                Age = 32,
                Address = new Address
                {
                    Street = "Isabella St",
                    Number = 23,
                    City = "Orlando",
                    State = "Florida",
                    Country = "United States",
                    ZipCode = 392982
                }
            },
            new()
            {
                FirstName = "Rodrick",
                LastName = "Ventura",
                Email = "rodrickventura@email.com",
                Identification = "F38321282",
                Phone = "1-543-824-8132",
                Age = 38,
                Address = new Address
                {
                    Street = "Damon Rd",
                    Number = 234,
                    City = "New Jersey",
                    State = "New York",
                    Country = "United States",
                    ZipCode = 564234
                }
            },
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@email.com",
                Identification = "X28492",
                Phone = "1-743-864-8221",
                Age = 40,
                Address = new Address
                {
                    Street = "Holly Dr",
                    Number = 390,
                    City = "Tampa",
                    State = "Florida",
                    Country = "United States",
                    ZipCode = 544233
                }
            },
            new()
            {
                FirstName = "Jane",
                LastName = "McGill",
                Email = "janemcgill@email.com",
                Identification = "P38478992",
                Phone = "1-395-422-6221",
                Age = 22,
                Address = new Address
                {
                    Street = "Olive Dr",
                    Number = 351,
                    City = "Montreal",
                    State = "Saskatchewan",
                    Country = "United States",
                    ZipCode = 392982
                }
            },
            new()
            {
                FirstName = "Dannie",
                LastName = "Backler",
                Email = "dbackler0@ebay.co.uk",
                Identification = "62-9003349",
                Phone = "1-301-441-6512",
                Age = 55,
                Address = new Address
                {
                    Street = "Ahum Dr",
                    Number = 493,
                    City = "La Paz",
                    State = "Montana",
                    Country = "United States",
                    ZipCode = 534221
                }
            },
            new()
            {
                FirstName = "Leslie",
                LastName = "Petrelluzzi",
                Email = "lpetrelluzzi1@com.com",
                Identification = "26-0468929",
                Phone = "1-341-451-5431",
                Age = 50,
                Address = new Address
                {
                    Street = "Osborn St",
                    Number = 211,
                    City = "Arlington",
                    State = "Johnston",
                    Country = "United States",
                    ZipCode = 988421
                }
            }
        };

        context.Patients.AddRange(patients);
        context.SaveChanges();

        var now = DateTime.Now;

        var appointments = new List<Appointment>
        {
            new()
            {
                Patient = patients[0],
                Doctor = doctors[0],
                Start = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0)
            },
            new()
            {
                Patient = patients[1],
                Doctor = doctors[1],
                Start = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0)
            },
            new()
            {
                Patient = patients[2],
                Doctor = doctors[2],
                Start = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0)
            },
            new()
            {
                Patient = patients[3],
                Doctor = doctors[3],
                Start = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0)
            },
            new()
            {
                Patient = patients[4],
                Doctor = doctors[4],
                Start = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0)
            },
            new()
            {
                Patient = patients[5],
                Doctor = doctors[5],
                Start = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0)
            },
            new()
            {
                Patient = patients[0],
                Doctor = doctors[6],
                Start = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 16, 0, 0)
            },
            new()
            {
                Patient = patients[1],
                Doctor = doctors[7],
                Start = new DateTime(now.Year, now.Month, now.Day, 16, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 17, 0, 0)
            },
            new()
            {
                Patient = patients[2],
                Doctor = doctors[8],
                Start = new DateTime(now.Year, now.Month, now.Day, 17, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0)
            },
            new()
            {
                Patient = patients[3],
                Doctor = doctors[9],
                Start = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0),
                End = new DateTime(now.Year, now.Month, now.Day, 19, 0, 0)
            }
        };

        context.Appointments.AddRange(appointments);
        context.SaveChanges();
    }
}
