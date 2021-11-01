namespace Ticketeer.Application.Contracts.Settings
{
    public interface IMonogDbSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}
