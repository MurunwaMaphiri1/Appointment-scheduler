using System.ComponentModel.DataAnnotations;

namespace AppointmentSchedulerAPI.Models.DTO;

public class EmailNotificationDto
{

    [Required]
    [StringLength(255)]
    public string SendTo { get; set; }

    [Required]
    public string MessageBody { get; set; }
    
}