using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using HelpDesk.Presentation.Models;

namespace HelpDesk.Presentation.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(_emailSettings.From));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                email.Body = new TextPart("plain")
                {
                    Text = body
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.Port,
                    SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(
                    _emailSettings.Username,
                    _emailSettings.Password);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }
    }
}