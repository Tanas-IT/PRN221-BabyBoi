using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BaByBoi.DataAccess.DTOs;
using BaByBoi.DataAccess.Service.Interface;

namespace BaByBoi.DataAccess.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_smtpSettings.From);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = _smtpSettings.EnableSsl;

                await client.SendMailAsync(message);
            }
        }
    }
}
