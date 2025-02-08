using AppointmentSchedulerAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSchedulerAPI.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentType> AppointmentTypes { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EmailNotification> EmailNotifications { get; set; }
    
    public DbSet<Timestamp> Timestamps { get; set; }
}