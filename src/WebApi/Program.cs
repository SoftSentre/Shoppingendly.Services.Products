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
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.App;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.CQRS;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Data;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Domain;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Logging;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Mappings;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Logger.Configuration;
using SoftSentre.Shoppingendly.Services.Products.WebApi.Endpoints;
using SoftSentre.Shoppingendly.Services.Products.WebApi.Modules;
using ILogger = Serilog.ILogger;

namespace SoftSentre.Shoppingendly.Services.Products.WebApi
{
    public class Program
    {
        private static ILogger _logger;
        private static IApplicationService _applicationService;

        public static async Task<int> Main(string[] args)
        {
            _applicationService = new ApplicationService();

            var environment = _applicationService.GetEnvironmentName();
            var appName = _applicationService.GetAppName();
            
            try
            {
                CreateSerilogLogger(environment);
                
                _logger.Information("Configuring web host ({Application})...", appName);
                
                var host = CreateHostBuilder(args).Build();
                host.MigrateDatabase<ProductServiceDbContext>((context, provider) =>
                {
                    var logger = provider.GetService<ILogger<ProductServiceDbContextSeed>>();
                
                    new ProductServiceDbContextSeed()
                        .SeedAsync(context, logger)
                        .Wait();
                });
                
                _logger.Information("Applying migrations ({Application})...", appName);

                await host.RunAsync();
                
                return 0;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Program terminated unexpectedly ({Application})!", appName);
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

        private static void CreateSerilogLogger(string environment)
        {
            ISerilogConfigurator serilogConfigurator = new SerilogConfigurator(_applicationService);
            var logger = serilogConfigurator.CreateSerilogLogger(environment);

            _logger = logger;
        }
    }
}