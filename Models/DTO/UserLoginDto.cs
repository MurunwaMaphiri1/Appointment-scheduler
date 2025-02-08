using System.ComponentModel.DataAnnotations;

namespace AppointmentSchedulerAPI.Models.DTO;

public class UserLoginDto
{
    [Required]
    [StringLength(255)]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}