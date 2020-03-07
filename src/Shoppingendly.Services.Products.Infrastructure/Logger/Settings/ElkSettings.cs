namespace Shoppingendly.Services.Products.Infrastructure.Logger.Settings
{
    public class ElkSettings
    {
        public bool Enabled { get; set; }
        public string LoggingLevel { get; set; }
        public string Url { get; set; }
        public bool BasicAuthEnabled { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string IndexFormat { get; set; }
    }
}