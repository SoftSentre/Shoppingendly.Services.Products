// Copyright 2020 SoftSentre Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Serilog;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Events;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Settings;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger.Configuration;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger.Settings;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Logger
{
    public class SerilogConfiguratorTests
    {
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

        private static IEnumerable<ILogEventFilter> GetFilters(Serilog.Core.Logger logger)
        {
            var aggregateSinkFieldInfo = logger.GetType()
                .GetField("_sink", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateSink = (ILogEventSink) aggregateSinkFieldInfo?.GetValue(logger);

            var filterEnumerableFieldInfo = aggregateSink?.GetType()
                .GetField("_filters", BindingFlags.Instance | BindingFlags.NonPublic);

            var filters = (ILogEventFilter[]) filterEnumerableFieldInfo?
                .GetValue(aggregateSink);

            return filters;
        }

        private static string GetPropertyEnricherName(IReadOnlyList<ILogEventEnricher> enrichers, int index)
        {
            return enrichers[index].GetType().GetField("_name", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(enrichers[index]).ToString();
        }

        private static string GetPropertyEnricherValue(IReadOnlyList<ILogEventEnricher> enrichers, int index)
        {
            return enrichers[index].GetType().GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(enrichers[index]).ToString();
        }

        private static string GetSinkFullName(IReadOnlyList<ILogEventSink> sinks, int index)
        {
            return sinks[index].GetType().GetField("_sink", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(sinks[index]).GetType().FullName;
        }

        private static LoggingLevelSwitch GetLoggingLevel(IReadOnlyList<ILogEventSink> sinks, int index)
        {
            return (LoggingLevelSwitch) sinks[index].GetType()
                .GetField("_levelSwitch", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(sinks[index]);
        }

        private static Serilog.Core.Logger CreateLoggerForTests(AppOptions appOptions = null,
            IEnumerable<string> excludePaths = null, IEnumerable<string> excludeProperties = null,
            string interval = "day")
        {
            ISerilogConfigurator serilogConfigurator = new SerilogConfigurator();
            var loggerConfiguration = new LoggerConfiguration();
            const string environment = "development";

            var loggerSettings = new LoggerSettings
            {
                FileSettings = new FileSettings
                {
                    Enabled = true,
                    Interval = interval,
                    LoggingLevel = "Information",
                    Path = "Logs/logs.txt"
                },
                ConsoleSettings = new ConsoleSettings
                {
                    Enabled = true,
                    LoggingLevel = "Debug"
                },
                ElkSettings = new ElkSettings
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
                },
                Tags = new Dictionary<string, object>
                {
                    {"Author", "tomato-sauce"}
                },
                ExcludePaths = excludePaths,
                ExcludeProperties = excludeProperties
            };

            var applicationOptions = appOptions ?? new AppOptions
            {
                Name = "Shoppingendly",
                Instance = "1",
                Service = "Products",
                Version = "1.0.0"
            };

            return serilogConfigurator
                .ConfigureLogger(loggerConfiguration, loggerSettings, applicationOptions, environment)
                .CreateLogger();
        }

        [Fact]
        public void CheckIfConfigureMethodSetCorrectEnrichersInSerilogConfiguration()
        {
            // Arrange
            var appOptions = new AppOptions
            {
                Name = "Shoppingendly",
                Instance = "1",
                Service = "Products",
                Version = "1.0.5"
            };

            var logger = CreateLoggerForTests(appOptions, interval: "wrong");

            // Act
            var enrichers = GetEnrichers(logger).ToList();

            // Assert
            var enricherDictionary = new Dictionary<string, string>
            {
                {GetPropertyEnricherName(enrichers, 0), GetPropertyEnricherValue(enrichers, 0)},
                {GetPropertyEnricherName(enrichers, 1), GetPropertyEnricherValue(enrichers, 1)},
                {GetPropertyEnricherName(enrichers, 2), GetPropertyEnricherValue(enrichers, 2)},
                {GetPropertyEnricherName(enrichers, 3), GetPropertyEnricherValue(enrichers, 3)},
                {GetPropertyEnricherName(enrichers, 4), GetPropertyEnricherValue(enrichers, 4)}
            };

            enrichers.Count.Should().Be(enricherDictionary.Count);
            enricherDictionary["Environment"].Should().Be("development");
            enricherDictionary["Application"].Should().Be(appOptions.Service);
            enricherDictionary["Instance"].Should().Be(appOptions.Instance);
            enricherDictionary["Version"].Should().Be(appOptions.Version);
            enricherDictionary["Author"].Should().Be("tomato-sauce");
        }

        [Fact]
        public void CheckIfConfigureMethodSetCorrectFiltersInSerilogConfiguration()
        {
            // Arrange
            var excludePaths = new[]
            {
                "/ping",
                "/metrics"
            };

            var excludeProperties = new[]
            {
                "api_key",
                "access_key",
                "ApiKey"
            };

            var logger = CreateLoggerForTests(excludePaths: excludePaths, excludeProperties: excludeProperties);

            // Act
            var filters = GetFilters(logger).ToArray();

            // Assert
            filters.Length.Should().Be(excludePaths.Length + excludeProperties.Length);
        }

        [Fact]
        public void CheckIfConfigureMethodSetCorrectSinksInSerilogConfiguration()
        {
            // Arrange
            var logger = CreateLoggerForTests();

            // Act
            var sinks = GetSinks(logger);

            // Assert
            sinks.Count.Should().Be(4);
            GetSinkFullName(sinks, 0).Should().Be("Serilog.Sinks.File.RollingFileSink");
            GetSinkFullName(sinks, 1).Should().Be("Serilog.Sinks.SystemConsole.ConsoleSink");
            GetSinkFullName(sinks, 2).Should().Be("Serilog.Sinks.Elasticsearch.ElasticsearchSink");
            GetSinkFullName(sinks, 3).Should().Be("Serilog.Sinks.Seq.SeqSink");
            GetLoggingLevel(sinks, 0).MinimumLevel.Should().Be(LogEventLevel.Information);
            GetLoggingLevel(sinks, 1).MinimumLevel.Should().Be(LogEventLevel.Debug);
            GetLoggingLevel(sinks, 2).MinimumLevel.Should().Be(LogEventLevel.Information);
            GetLoggingLevel(sinks, 3).MinimumLevel.Should().Be(LogEventLevel.Information);
        }
    }
}