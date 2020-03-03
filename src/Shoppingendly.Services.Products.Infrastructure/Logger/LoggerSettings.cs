using System.Collections.Generic;
using Shoppingendly.Services.Products.Infrastructure.Logger.Settings;

namespace Shoppingendly.Services.Products.Infrastructure.Logger
{
    public class LoggerSettings
    {
        public FileSettings FileSettings { get; set; }
        public ConsoleSettings ConsoleSettings { get; set; }
        public ElkSettings ElkSettings { get; set; }
        public SeqSettings SeqSettings { get; set; }
        public IEnumerable<string> ExcludePaths { get; set; }
        public IEnumerable<string> ExcludeProperties { get; set; }
        public IDictionary<string, object> Tags { get; set; }
    }
}