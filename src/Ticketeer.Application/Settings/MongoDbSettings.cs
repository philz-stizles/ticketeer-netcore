using Ticketeer.Application.Contracts.Settings;

namespace Ticketeer.Application.Settings
{
    public class MongoDbSettings: IMonogDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
