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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework
{
    public class ProductServiceDbContextSeed
    {
        public async Task SeedAsync(ProductServiceDbContext context, ILogger<ProductServiceDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(ProductServiceDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                await using (context)
                {
                    await context.Database.MigrateAsync();
                }
            });
        }

        private static AsyncRetryPolicy CreatePolicy(ILogger logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix, exception.GetType().Name, exception.Message, retry, retries);
                }
            );
        }
    }
}