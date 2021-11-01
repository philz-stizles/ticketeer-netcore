using System.IO;
using System.Threading.Tasks;

namespace Ticketeer.Application.Contracts.Infrastructure.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string Subject, string Email, string Body);
        Task SendEmailWithAttachmentAsync(string Subject, string Email, string Body,
           Stream attachmentStream, string FileName
       );

        Task SendHTMLEmailAsync(string Subject, string Email, string Body);
    }
}
