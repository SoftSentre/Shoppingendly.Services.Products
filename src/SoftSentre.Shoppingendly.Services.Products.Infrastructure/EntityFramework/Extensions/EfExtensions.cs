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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Extensions
{
    public static class EfExtensions
    {
        public static void UseLogging(this DbContextOptionsBuilder dbContextOptionsBuilder,
            ILoggerFactory loggerFactory) =>
            dbContextOptionsBuilder.UseLoggerFactory(loggerFactory)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        
        public static DbContextOptionsBuilder UseStronglyTypedIds(this DbContextOptionsBuilder dbContextOptionsBuilder)
            => dbContextOptionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
    }
}