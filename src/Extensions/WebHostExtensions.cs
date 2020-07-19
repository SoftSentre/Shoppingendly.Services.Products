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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace SoftSentre.Shoppingendly.Services.Products.Extensions
{
    public static class WebHostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,
            Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}",
                    typeof(TContext).Name);

                var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(new TimeSpan[]
                    {
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(8),
                    });

                retry.Execute(() =>
                {
                    context.Database.Migrate();
                    seeder(context, services);
                });

                logger.LogInformation("Migrated database associated with context {DbContextName}",
                    typeof(TContext).Name);
            }
            catch (Exception exception)
            {
                logger.LogError(exception,
                    "An error occurred while migrating the database used on context {DbContextName}",
                    typeof(TContext).Name);
            }

            return host;
        }
    }
}