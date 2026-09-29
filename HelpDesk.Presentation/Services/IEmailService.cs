using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Presentation.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}