namespace Shoppingendly.Services.Products.Infrastructure.Logger.Settings
{
    public class FileSettings
    {
        public bool Enabled { get; set; }
        public string LoggingLevel { get; set; }
        public string Path { get; set; }
        public string Interval { get; set; }
    }
}