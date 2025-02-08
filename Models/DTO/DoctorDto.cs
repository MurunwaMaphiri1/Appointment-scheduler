using System.ComponentModel.DataAnnotations;

namespace AppointmentSchedulerAPI.Models.DTO;

public class DoctorDto
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    public string Role { get; set; }

    public string ImagePath { get; set; }
}