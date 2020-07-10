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
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.CQRS;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Data;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Domain;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Logging;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Mappings;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger.Configuration;
using SoftSentre.Shoppingendly.Services.Products.WebApi.Endpoints;
using SoftSentre.Shoppingendly.Services.Products.WebApi.Modules;
using ILogger = Serilog.ILogger;

namespace SoftSentre.Shoppingendly.Services.Products.WebApi
{
    public class Program
    {
        private static ILogger _logger;
        private static string _appName;

        public static async Task<int> Main(string[] args)
        {
            var configuration = GetConfiguration();
            var environment = GetEnvironment();
            
            try
            {
                CreateSerilogLogger(configuration, environment);
                
                await CreateHostBuilder(args).Build().RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", _appName);
                return 1;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((app, containerBuilder) =>
                {
                    containerBuilder.RegisterModule<CqrsModule>();
                    containerBuilder.RegisterModule<DomainModule>();
                    containerBuilder.RegisterModule<CommandHandlersModule>();
                    containerBuilder.RegisterModule<QueryHandlersModule>();

                    containerBuilder.RegisterModule(
                        new DataAccessModule(
                            new SerilogLoggerFactory(_logger)));

                    containerBuilder.RegisterModule<DomainEventHandlersModule>();
                    containerBuilder.RegisterModule<MappingModule>();
                    containerBuilder.RegisterModule<LoggingModule>();
                    containerBuilder.RegisterModule<DomainEventsModule>();
                    containerBuilder.RegisterModule<EndpointInvokersModule>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services => { services.AddControllers(); });
                    webBuilder.Configure(app =>
                    {
                        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

                        if (env.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }

                        app.UseHttpsRedirection();
                        app.UseRouting();
                        app.UseAuthorization();
                        
                        var endpointInvoker = app.ApplicationServices.GetService<CreatorEndpointsInvoker>();
                        
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();

                            endpoints.MapGet("/",
                                async context =>
                                    await context.Response.WriteAsync("Hello it's Shoppingendly Product Service."));
                            
                            endpoints.MapGet("api/products/creators/{creatorId}", async context =>
                                await endpointInvoker.GetCreatorProducts(context));

                        });
                    });
                });
        }

        private static void CreateSerilogLogger(IConfiguration configuration, string environment)
        {
            var loggerSettings = configuration.GetOptions<LoggerSettings>("logger") ?? new LoggerSettings();
            var appOptions = configuration.GetOptions<AppOptions>("app") ?? new AppOptions();
            _appName = appOptions.Name;

            ISerilogConfigurator loggerConfigurator = new SerilogConfigurator();
            var loggerConfiguration = new LoggerConfiguration();
            var filledConfiguration =
                loggerConfigurator.ConfigureLogger(loggerConfiguration, loggerSettings, appOptions, environment);

            _logger = filledConfiguration.CreateLogger();
        }

        private static string GetEnvironment()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment.IsEmpty())
                throw new ArgumentNullException(nameof(environment),
                    "Unable to run application without specified environment.");

            return environment;
        }
        
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}