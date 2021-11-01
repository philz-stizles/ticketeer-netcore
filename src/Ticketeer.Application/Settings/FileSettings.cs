using System.Collections.Generic;

namespace Ticketeer.Application.Settings
{
    public class FileSettings
    {
        public IList<string> AllowedFileTypes { get; set; }
        public int MaxBytes { get; set; }
        public UploadFolderSettings UploadFolders { get; set; }
    }

    public class UploadFolderSettings
    {
        public string Documents { get; set; }
        public string Images { get; set; }
    }
}