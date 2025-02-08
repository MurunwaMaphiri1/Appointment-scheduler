using System.Text.Json;
using AppointmentSchedulerAPI.Data;
using AppointmentSchedulerAPI.Models.DTO;
using AppointmentSchedulerAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSchedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public AppointmentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
        }

        //Get all Appointments
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var allAppointments = _context.Appointments.ToList();
            return Ok(allAppointments);
        }

        //Get Appointment by id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = _context.Appointments.FindAsync(id);
            return Ok(appointment);
        }
        
        //Get appointments by userId
        // [HttpGet]
        // [Route("{id:int}/users")]

        //Add Appointment
        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentDto appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointmentEntity = new Appointment()
            {
                Title = appointment.Title,
                UserId = appointment.UserId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate.Date,
                AppointmentTime = appointment.AppointmentTime,
                Status = Enum.Parse<AppointmentStatus>(appointment.Status),
            };
            
            _context.Appointments.Add(appointmentEntity);
            await _context.SaveChangesAsync();

            var response = new
            {
                appointmentEntity.Id,
                appointmentEntity.Title,
                appointmentEntity.UserId,
                appointmentEntity.DoctorId,
                CreatedDate = appointmentEntity.CreatedDate.ToString("yyyy-MM-dd"),
                // CreatedDate = appointmentEntity.CreatedDate.ToString("yyyy-MM-dd"),
                AppointmentDate = appointmentEntity.AppointmentDate.ToString("yyyy-MM-dd"),
                AppointmentTime = appointmentEntity.AppointmentTime.ToString(@"hh\:mm"),
                Status = appointmentEntity.Status.ToString(),
            };
            

            return Ok(response);
        }


        //Delete Appointment
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

            //For debugging purposes
            return Ok(appointment);

            //return NoContent();
        }

        //Update Appointment
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDto appointment)
        {
            var appointmentEntity = await _context.Appointments.FindAsync(id);

            if (appointmentEntity == null)
            {
                return NotFound();
            }

            appointmentEntity.Title = appointment.Title;
            appointmentEntity.DoctorId = appointment.DoctorId;
            appointmentEntity.AppointmentDate = appointment.AppointmentDate;
            appointmentEntity.AppointmentTime = appointment.AppointmentTime;

            await _context.SaveChangesAsync();

            //For debugging purposes
            return Ok(appointmentEntity);

            //return NoContent();
        }
    }
}

