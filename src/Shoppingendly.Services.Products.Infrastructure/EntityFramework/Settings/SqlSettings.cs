namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings
{
    public class SqlSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool UseInMemory { get; set; }
    }
}