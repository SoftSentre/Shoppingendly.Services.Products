using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Serilog;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Shoppingendly.Services.Products.Infrastructure.Logger;
using Shoppingendly.Services.Products.Infrastructure.Logger.Configuration;
using Shoppingendly.Services.Products.Infrastructure.Logger.Settings;
using Shoppingendly.Services.Products.Infrastructure.Options;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Logger
{
    public class SerilogConfiguratorTests
    {
        [Fact]
        public void CheckIfConfigureMethodSetCorrectValuesInSerilogConfiguration()
        {
            // Arrange
            ISerilogConfigurator serilogConfigurator = new SerilogConfigurator();
            var loggerConfiguration = new LoggerConfiguration();
            const string environment = "development";
            
            var loggerSettings = new LoggerSettings()
            {
                FileSettings = new FileSettings
                {
                    Enabled = true,
                    Interval = "day",
                    LoggingLevel = "Information",
                    Path = "Logs/logs.txt"
                },
                ConsoleSettings = new ConsoleSettings
                {
                    Enabled = true,
                    LoggingLevel = "Debug"
                },
                ElkSettings = new ElkSettings()
                {
                    Enabled = true,
                    BasicAuthEnabled = false,
                    LoggingLevel = "Information",
                    Url = "http://localhost:8000"
                },
                SeqSettings = new SeqSettings
                {
                    Enabled = true,
                    Url = "http://localhost:5341",
                    ApiKey = "secret"
                }
            };

            var appOptions = new AppOptions
            {
                Application = "Shoppingendly",
                Instance = "1",
                Service = "Products",
                Version = "1.0.0"
            };

            // Act
            var logger = serilogConfigurator
                .ConfigureLogger(loggerConfiguration, loggerSettings, appOptions, environment)
                .CreateLogger();

            // Assert
            var sinks = GetSinks(logger);
            var enrichers = GetEnrichers(logger).ToList();

            var fileSink = GetSinkFullName(sinks, 0);
            var consoleSink = GetSinkFullName(sinks, 1);
            var elkSink = GetSinkFullName(sinks, 2);
            var seqSink = GetSinkFullName(sinks, 3);

            var fileMinimumLoggingLevel = GetLoggingLevel(sinks, 0).MinimumLevel;
            var consoleMinimumLoggingLevel = GetLoggingLevel(sinks, 1).MinimumLevel;
            var elkMinimumLoggingLevel = GetLoggingLevel(sinks, 2).MinimumLevel;
            var seqMinimumLoggingLevel = GetLoggingLevel(sinks, 3).MinimumLevel;

            var enricherDictionary = new Dictionary<string, string>()
            {
                {GetPropertyEnricherName(enrichers, 0), GetPropertyEnricherValue(enrichers, 0)},
                {GetPropertyEnricherName(enrichers, 1), GetPropertyEnricherValue(enrichers, 1)},
                {GetPropertyEnricherName(enrichers, 2), GetPropertyEnricherValue(enrichers, 2)},
                {GetPropertyEnricherName(enrichers, 3), GetPropertyEnricherValue(enrichers, 3)},
            };

            sinks.Count.Should().Be(4);

            fileSink.Should().Be("Serilog.Sinks.File.RollingFileSink");
            consoleSink.Should().Be("Serilog.Sinks.SystemConsole.ConsoleSink");
            elkSink.Should().Be("Serilog.Sinks.Elasticsearch.ElasticsearchSink");
            seqSink.Should().Be("Serilog.Sinks.Seq.SeqSink");

            fileMinimumLoggingLevel.Should().Be(LogEventLevel.Information);
            consoleMinimumLoggingLevel.Should().Be(LogEventLevel.Debug);
            elkMinimumLoggingLevel.Should().Be(LogEventLevel.Information);
            seqMinimumLoggingLevel.Should().Be(LogEventLevel.Information);

            enricherDictionary["Environment"].Should().Be(environment);
            enricherDictionary[nameof(appOptions.Application)].Should().Be(appOptions.Service);
            enricherDictionary[nameof(appOptions.Instance)].Should().Be(appOptions.Instance);
            enricherDictionary[nameof(appOptions.Version)].Should().Be(appOptions.Version);
        }

        private static IReadOnlyList<ILogEventSink> GetSinks(Serilog.Core.Logger logger)
        {
            var aggregateSinkFieldInfo = logger.GetType()
                .GetField("_sink", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateSink = (ILogEventSink) aggregateSinkFieldInfo?.GetValue(logger);

            var sinkEnumerableFieldInfo = aggregateSink?.GetType()
                .GetField("_sinks", BindingFlags.Instance | BindingFlags.NonPublic);

            var sinks = (ILogEventSink[]) sinkEnumerableFieldInfo?
                .GetValue(aggregateSink);

            return sinks;
        }

        private static IEnumerable<ILogEventEnricher> GetEnrichers(Serilog.Core.Logger logger)
        {
            var aggregateEnricherFieldInfo = logger.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = (ILogEventEnricher) aggregateEnricherFieldInfo?.GetValue(logger);

            var enricherEnumerableFieldInfo = aggregateEnricher?.GetType()
                .GetField("_enrichers", BindingFlags.Instance | BindingFlags.NonPublic);

            var enrichers = (ILogEventEnricher[]) enricherEnumerableFieldInfo?
                .GetValue(aggregateEnricher);

            var propertyEnrichers = enrichers?.Where(x => x is PropertyEnricher);

            return propertyEnrichers;
        }

        private static string GetPropertyEnricherName(IReadOnlyList<ILogEventEnricher> enrichers, int index)
            => enrichers[index].GetType().GetField("_name", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(enrichers[index]).ToString();

        private static string GetPropertyEnricherValue(IReadOnlyList<ILogEventEnricher> enrichers, int index)
            => enrichers[index].GetType().GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(enrichers[index]).ToString();

        private static string GetSinkFullName(IReadOnlyList<ILogEventSink> sinks, int index)
            => sinks[index].GetType().GetField("_sink", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(sinks[index]).GetType().FullName;

        private static LoggingLevelSwitch GetLoggingLevel(IReadOnlyList<ILogEventSink> sinks, int index)
            => (LoggingLevelSwitch) sinks[index].GetType()
                .GetField("_levelSwitch", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(sinks[index]);
    }
}