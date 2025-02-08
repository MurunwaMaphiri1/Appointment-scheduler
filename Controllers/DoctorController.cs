using AppointmentSchedulerAPI.Data;
using AppointmentSchedulerAPI.Models.DTO;
using AppointmentSchedulerAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSchedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController: Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            this._context = context;
        }
        
        //Get all Doctors
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var allDoctors = _context.Doctors.ToList();
            return Ok(allDoctors);
        }

        //Get Doctor by id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            return Ok(doctor);
        }

        //Add Doctor
        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorDto doctor)
        {
            var doctorEntity = new Doctor()
            {
                Name = doctor.Name,
                Role = doctor.Role,
                ImagePath = doctor.ImagePath,
            };

            _context.Doctors.Add(doctorEntity);
            _context.SaveChanges();
            return Ok(doctor);
        }

        //Delete Doctor
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return Ok(doctor);
        }

        //Update doctor
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateDoctor(int id, DoctorDto doctor)
        {
            var doctorEntity = await _context.Doctors.FindAsync(id);
            doctorEntity.Name = doctor.Name;
            doctorEntity.Role = doctor.Role;
            doctorEntity.ImagePath = doctor.ImagePath;
            _context.Doctors.Update(doctorEntity);
            await _context.SaveChangesAsync();
            return Ok(doctorEntity);
        }
    }    
}
