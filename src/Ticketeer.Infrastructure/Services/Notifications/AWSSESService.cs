using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Infrastructure.Services;
using Ticketeer.Application.Settings;

namespace Ticketeer.Infrastructure.Services.Notifications
{
    public class AWSSESService: IEmailService
    {
        private readonly AppSettings _appSettings;
        private readonly AWSSESSettings _aWSSESSettings;
        private readonly IWebHostEnvironment _hostingEnv;

        public AWSSESService(IOptions<AppSettings> appSettings, IOptions<AWSSESSettings> aWSSESSettings,
            IWebHostEnvironment hostingEnv)
        {
            _appSettings = appSettings.Value;
            _aWSSESSettings = aWSSESSettings.Value;
            _hostingEnv = hostingEnv;
        }

        public async Task SendEmailAsync(string Subject, string Email, string Body)
        {
            string FROM = _appSettings.FromMail;
            string FROMNAME = "TICKETEER";
            string TO = Email;
            string SMTP_USERNAME = _aWSSESSettings.SMTPUsername;
            string SMTP_PASSWORD = _aWSSESSettings.SMTPPassword;
            string HOST = _aWSSESSettings.SMTPHost;
            int PORT = Convert.ToInt32(_aWSSESSettings.SMTPPort);
            string SUBJECT = Subject;
            string BODY = Body;

            using (MailMessage message = new MailMessage(FROMNAME + " " + FROM, TO))
            using (var client = new SmtpClient(HOST, PORT))
            {
                message.IsBodyHtml = true;
                message.Subject = SUBJECT;
                message.Body = BODY;

                client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }
        }

        public async Task SendHTMLEmailAsync(string Subject, string Email, string Body)
        {
            string FROM = _appSettings.AdminMail;
            string FROMNAME = "TICKETEER";
            string TO = Email;
            string SMTP_USERNAME = _aWSSESSettings.SMTPUsername;
            string SMTP_PASSWORD = _aWSSESSettings.SMTPPassword;
            string HOST = _aWSSESSettings.SMTPHost;
            int PORT = Convert.ToInt32(_aWSSESSettings.SMTPPort);
            string SUBJECT = Subject;
            string BODY = Body;

            using (MailMessage message = new MailMessage(FROMNAME + " " + FROM, TO))
            using (var client = new SmtpClient(HOST, PORT))
            {
                message.IsBodyHtml = true;
                message.Subject = SUBJECT;
                message.Body = BODY;

                client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }
        }

        public async Task SendEmailWithAttachmentAsync(string Subject, string Email,
            string Body, Stream attachmentStream, string FileName
        )
        {
            string FROM = _appSettings.FromMail;
            string FROMNAME = "TICKETEER";
            string TO = Email;
            string SMTP_USERNAME = _aWSSESSettings.SMTPUsername;
            string SMTP_PASSWORD = _aWSSESSettings.SMTPPassword;
            string HOST = _aWSSESSettings.SMTPHost;
            int PORT = Convert.ToInt32(_aWSSESSettings.SMTPPort);
            string SUBJECT = Subject;
            string BODY = Body;

            using (MailMessage message = new MailMessage(FROMNAME + " " + FROM, TO))
            using (var client = new SmtpClient(HOST, PORT))
            {
                message.IsBodyHtml = true;
                message.Subject = SUBJECT;
                message.Body = BODY;

                // Create the file attachment for this email message.
                Attachment data = new Attachment(attachmentStream, FileName, MediaTypeNames.Application.Octet);

                // Add the file attachment to this email message.
                message.Attachments.Add(data);

                client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                client.EnableSsl = true;
                await client.SendMailAsync(message);
            }
        }
    }
}
