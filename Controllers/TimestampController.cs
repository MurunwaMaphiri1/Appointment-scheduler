using AppointmentSchedulerAPI.Data;
using AppointmentSchedulerAPI.Models.DTO;
using AppointmentSchedulerAPI.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSchedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimestampController: Controller
    {
        private readonly ApplicationDbContext _context;
        
        private readonly IConfiguration _configuration;

        public TimestampController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        // Get all Timestamps
         [HttpGet]
         public async Task<IActionResult> GetTimestamp()
         {
             var allTimestamps = _context.Timestamps.ToList();
                return Ok(allTimestamps);
         }
    }    
}
