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

using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Elasticsearch;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger.Settings;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger.Configuration
{
    public class SerilogConfigurator : ISerilogConfigurator
    {
        public LoggerConfiguration ConfigureLogger(LoggerConfiguration loggerConfiguration,
            LoggerSettings loggerSettings, AppOptions appOptions, string environmentName)
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

            return loggerConfiguration;
        }

        private static void ConfigureFileLogger(FileSettings fileSettings, LoggerConfiguration loggerConfiguration)
        {
            var settings = fileSettings ?? new FileSettings();

            if (!settings.Enabled)
            {
                return;
            }

            var path = string.IsNullOrWhiteSpace(settings.Path) ? "logs/logs.txt" : settings.Path;
            var level = ParseLoggingLevel(settings.LoggingLevel);

            if (!Enum.TryParse<RollingInterval>(settings.Interval, true, out var interval))
            {
                interval = RollingInterval.Day;
            }

            loggerConfiguration.WriteTo.File(path, level, rollingInterval: interval);
        }

        private static void ConfigureConsoleLogger(ConsoleSettings consoleSettings,
            LoggerConfiguration loggerConfiguration)
        {
            var settings = consoleSettings ?? new ConsoleSettings();

            if (!settings.Enabled)
            {
                return;
            }

            var level = ParseLoggingLevel(settings.LoggingLevel);

            loggerConfiguration.WriteTo.Console(level);
        }

        private static void ConfigureElkStack(ElkSettings elkSettings, LoggerConfiguration loggerConfiguration)
        {
            var settings = elkSettings ?? new ElkSettings();

            if (!settings.Enabled)
            {
                return;
            }

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

            if (!settings.Enabled)
            {
                return;
            }

            var level = ParseLoggingLevel(settings.LoggingLevel);

            loggerConfiguration.WriteTo.Seq(settings.Url, level, apiKey: settings.ApiKey);
        }

        private static LogEventLevel ParseLoggingLevel(string loggingLevel)
        {
            if (!Enum.TryParse<LogEventLevel>(loggingLevel, true, out var level))
            {
                level = LogEventLevel.Information;
            }

            return level;
        }
    }
}