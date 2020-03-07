using Serilog;
using Shoppingendly.Services.Products.Infrastructure.Options;

namespace Shoppingendly.Services.Products.Infrastructure.Logger.Configuration
{
    public interface ISerilogConfigurator
    {
        LoggerConfiguration ConfigureLogger(LoggerConfiguration loggerConfiguration, LoggerSettings loggerSettings,
            AppOptions appOptions, string environmentName);
    }
}