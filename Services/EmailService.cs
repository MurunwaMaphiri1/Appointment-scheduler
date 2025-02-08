using System.Net;
using System.Net.Mail;
using AppointmentSchedulerAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using AppointmentSchedulerAPI.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using DotNetEnv;

namespace AppointmentSchedulerAPI.Services
{
    
    public interface IEmailService
    {
        Task SendEmail(EmailNotificationDto emailNotificationDto);
    }
    [Route("api/[controller]")]
    [ApiController]
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;
        
        public EmailService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost("send-email")]
        public async Task SendEmail(EmailNotificationDto emailNotificationDto)
        {
            DotNetEnv.Env.Load();
            var email = Environment.GetEnvironmentVariable("EMAIL");
            var password = Environment.GetEnvironmentVariable("PASSWORD");
            var host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(email, password);

            var subject = "Appointment Notification";

            var message = new MailMessage(email, emailNotificationDto.SendTo, subject, emailNotificationDto.MessageBody)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }    
}
