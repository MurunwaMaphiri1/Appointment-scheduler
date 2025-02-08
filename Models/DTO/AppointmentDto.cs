using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSchedulerAPI.Models.DTO;

public class AppointmentDto
{
    public string Title { get; set; }
    public int UserId { get; set; } 
    public int DoctorId { get; set; }
    
    public DateTime AppointmentDate { get; set; }
    
    public TimeSpan AppointmentTime { get; set; }
    public required string Status { get; set; }
}