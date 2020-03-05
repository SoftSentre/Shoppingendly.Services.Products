using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Elasticsearch;
using Shoppingendly.Services.Products.Infrastructure.Logger.Settings;
using Shoppingendly.Services.Products.Infrastructure.Options;

namespace Shoppingendly.Services.Products.Infrastructure.Logger.Configuration
{
    public class SerilogConfiguration : ISerilogConfiguration
    {
        public void ConfigureLogger(LoggerConfiguration loggerConfiguration, LoggerSettings loggerSettings,
            AppOptions appOptions, string environmentName)
        {
            loggerConfiguration
                .Enrich
                .FromLogContext()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", appOptions.Service)
                .Enrich.WithProperty("Instance", appOptions.Instance)
                .Enrich.WithProperty("Version", appOptions.Version);

            foreach (var (key, value) in loggerSettings.Tags ?? new Dictionary<string, object>())
            {
                loggerConfiguration.Enrich.WithProperty(key, value);
            }

            loggerSettings.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

            loggerSettings.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty(p)));

            ConfigureFileLogger(loggerSettings.FileSettings, loggerConfiguration);
            ConfigureConsoleLogger(loggerSettings.ConsoleSettings, loggerConfiguration);
            ConfigureElkStack(loggerSettings.ElkSettings, loggerConfiguration);
            ConfigureSeq(loggerSettings.SeqSettings, loggerConfiguration);
        }

        private static void ConfigureFileLogger(FileSettings fileSettings, LoggerConfiguration loggerConfiguration)
        {
            var settings = fileSettings ?? new FileSettings();

            if (!settings.Enabled) return;

            var path = string.IsNullOrWhiteSpace(settings.Path) ? "logs/logs.txt" : settings.Path;
            var level = ParseLoggingLevel(settings.LoggingLevel);

            if (!Enum.TryParse<RollingInterval>(settings.Interval, true, out var interval))
                interval = RollingInterval.Day;

            loggerConfiguration.WriteTo.File(path, level, rollingInterval: interval);
        }

        private static void ConfigureConsoleLogger(ConsoleSettings consoleSettings, LoggerConfiguration loggerConfiguration)
        {
            var settings = consoleSettings ?? new ConsoleSettings();

            if (!settings.Enabled) return;
            var level = ParseLoggingLevel(settings.LoggingLevel);

            loggerConfiguration.WriteTo.Console(level);
        }

        private static void ConfigureElkStack(ElkSettings elkSettings, LoggerConfiguration loggerConfiguration)
        {
            var settings = elkSettings ?? new ElkSettings();

            if (!settings.Enabled) return;
            var level = ParseLoggingLevel(settings.LoggingLevel);

            loggerConfiguration.WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(
                    new Uri(settings.Url))
                {
                    MinimumLogEventLevel = level,
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = string.IsNullOrWhiteSpace(settings.IndexFormat)
                        ? "logstash-{0:yyyy.MM.dd}"
                        : settings.IndexFormat,
                    ModifyConnectionSettings = connectionConfiguration =>
                        settings.BasicAuthEnabled
                            ? connectionConfiguration.BasicAuthentication(settings.Username, settings.Password)
                            : connectionConfiguration
                });
        }

        private static void ConfigureSeq(SeqSettings seqSettings, LoggerConfiguration loggerConfiguration)
        {
            var settings = seqSettings ?? new SeqSettings();

            if (!settings.Enabled) return;
            var level = ParseLoggingLevel(settings.LoggingLevel);

            loggerConfiguration.WriteTo.Seq(settings.Url, level, apiKey: settings.ApiKey);
        }

        private static LogEventLevel ParseLoggingLevel(string loggingLevel)
        {
            if (!Enum.TryParse<LogEventLevel>(loggingLevel, true, out var level))
                level = LogEventLevel.Information;

            return level;
        }
    }
}