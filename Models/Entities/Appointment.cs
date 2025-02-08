using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSchedulerAPI.Models.Entities;
public enum AppointmentStatus
{
    Pending,
    Completed
}
public class Appointment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "date")]
    public DateTime AppointmentDate { get; set; }

    [Required]
    [Column(TypeName = "time")]
    public TimeSpan AppointmentTime { get; set; }

    [Required]
    [Column(TypeName = "varchar(24)")]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    [ForeignKey("DoctorId")]
    public Doctor Doctor { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}