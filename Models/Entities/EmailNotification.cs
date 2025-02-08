using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppointmentSchedulerAPI.Models.Entities;

public class EmailNotification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    public int AppointmentId { get; set; }

    [Required]
    [StringLength(255)]
    public string SendTo { get; set; }

    [Required]
    public string MessageBody { get; set; }
    
    public DateTime SendDateTime { get; set; }

    [ForeignKey("AppointmentId")]
    public Appointment Appointment { get; set; }
}