using System;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shoppingendly.Services.Products.Infrastructure.Extensions;
using Shoppingendly.Services.Products.Infrastructure.Logger.Configuration;
using Shoppingendly.Services.Products.Infrastructure.Options;

namespace Shoppingendly.Services.Products.Infrastructure.Logger.Extensions
{
    public static class Registration
    {
        private const string AppSectionName = "app";
        private const string LoggerSectionName = "logger";

        public static IHostBuilder UseLogging(this IHostBuilder webHostBuilder,
            Action<LoggerConfiguration> configure = null)
        {
            var builder = webHostBuilder
                .UseSerilog((context, loggerConfiguration) =>
            {
                ISerilogConfigurator serilogConfiguration = new SerilogConfigurator();
                var loggerOptions = context.Configuration.GetOptions<LoggerSettings>(LoggerSectionName);
                var appOptions = context.Configuration.GetOptions<AppOptions>(AppSectionName);

                serilogConfiguration.ConfigureLogger(loggerConfiguration, loggerOptions, appOptions,
                    context.HostingEnvironment.EnvironmentName);
                configure?.Invoke(loggerConfiguration);
            });

            return builder;
        }
    }
}