using AppointmentSchedulerAPI.Data;
using AppointmentSchedulerAPI.Models.DTO;
using AppointmentSchedulerAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSchedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AppointmentTypesController: Controller
    {
        private readonly ApplicationDbContext _context;
        
        private readonly IConfiguration _configuration;

        public AppointmentTypesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        //Get appointment types
        [HttpGet]
        public async Task<IActionResult> GetAppointmentTypes()
        {
            var getAllAppointmentTypes = _context.AppointmentTypes.ToList();
            return Ok(getAllAppointmentTypes);
        }
    }
}    
