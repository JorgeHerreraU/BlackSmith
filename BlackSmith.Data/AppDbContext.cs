using BlackSmith.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmith.Data;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<WorkingDay> WorkTimes { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
