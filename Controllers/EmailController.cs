using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentSchedulerAPI.Data;
using AppointmentSchedulerAPI.Models.DTO;
using AppointmentSchedulerAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AppointmentSchedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController: Controller
{ 
        private readonly ApplicationDbContext _context;

        public EmailController(ApplicationDbContext context)
        {
            this._context = context;
        }

        //Get all Emails
        [HttpGet]
        public async Task<IActionResult> GetAllEmails()
        {
            var allEmails = _context.EmailNotifications.ToList();
            return Ok(allEmails);
        }

        //Get Email by id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetEmailById(int id)
        {
            var email = _context.EmailNotifications.FindAsync(id);
            return Ok(email);
        }

        //Add email
        [HttpPost]
        public async Task<IActionResult> AddEmail(EmailNotificationDto email)
        {
            var emailEntity = new EmailNotification()
            {
                SendTo = email.SendTo,
                MessageBody = email.MessageBody,
            };

            _context.EmailNotifications.Add(emailEntity);
            _context.SaveChanges();
            return Ok(email);
        }

        //Delete Email
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteEmail(int id)
        {
            var email = await _context.EmailNotifications.FindAsync(id);
            _context.EmailNotifications.Remove(email);
            await _context.SaveChangesAsync();
            return Ok(email);
        }

        //Update doctor
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateEmail(int id, EmailNotificationDto email)
        {
            var emailEntity = await _context.EmailNotifications.FindAsync(id);
            emailEntity.SendTo = email.SendTo;
            emailEntity.MessageBody = email.MessageBody;
            _context.EmailNotifications.Update(emailEntity);
            await _context.SaveChangesAsync();
            return Ok(emailEntity);
        }
}    

}