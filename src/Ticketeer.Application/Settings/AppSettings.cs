namespace Ticketeer.Application.Settings
{
    public class AppSettings
    {
        public string ApiUri { get; set; }
        public string WebUri { get; set; }
        public string[] CorsUrls { get; set; }
        public string AdminMail { get; set; }
        public string FromMail { get; set; }
    }
}
