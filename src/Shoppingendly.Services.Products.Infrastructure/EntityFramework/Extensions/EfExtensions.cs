using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Extensions
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