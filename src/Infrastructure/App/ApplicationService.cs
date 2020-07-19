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
using System.IO;
using Microsoft.Extensions.Configuration;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.App
{
    public class ApplicationService : IApplicationService
    {
        private const string AppSectionName = "app";
        private const string LoggerSectionName = "logger";
        private const string SqlSectionName = "sqlSettings";
        
        private IConfiguration Configuration { get; set; }
        
        public IConfiguration GetConfiguration()
        {
            Configuration =  new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return Configuration;
        }

        public string GetEnvironmentName()
        {
            var environment = GetEnvironment();

            if (environment.IsEmpty())
                throw new ArgumentException("Environment hasn't been specified.");

            return environment;
        }

        public string GetAppName()
        {
            var appOptions = GetAppSettings();

            return appOptions == null ? "Unknown" : appOptions.Name;
        }

        public AppOptions GetAppSettings()
        {
            var appSettings = GetSettings<AppOptions>(AppSectionName) ?? new AppOptions();

            return appSettings;
        }

        public LoggerSettings GetLoggerSettings()
        {
            var loggerSettings = GetSettings<LoggerSettings>(LoggerSectionName) ?? new LoggerSettings();

            return loggerSettings;
        }
        
        public SqlSettings GetSqlSettings()
        {
            var sqlSettings = GetSettings<SqlSettings>(SqlSectionName) ?? new SqlSettings();

            return sqlSettings;
        }

        private static string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        
        private TSettings GetSettings<TSettings>(string settingName) where TSettings : new()
        {
            var configuration = Configuration ?? GetConfiguration();

            return configuration.GetOptions<TSettings>(settingName);
        }
    }
}