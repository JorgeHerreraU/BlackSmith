using BlackSmith.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BlackSmith.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<WorkingDay> WorkTimes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasMany(d => d.WorkingDays).WithOne().OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
}