using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;
using Shoppingendly.Services.Products.Infrastructure.Extensions;

namespace Shoppingendly.Services.Products.Infrastructure.InversionOfControl.Modules
{
    public class DataAccessModule : Module
    {
        private readonly ILoggerFactory _loggerFactory;

        public DataAccessModule(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory
                .IfEmptyThenThrowAndReturnValue();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryEfRepository>()
                .As<ICategoryRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CreatorEfRepository>()
                .As<ICreatorRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductEfRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.Register(context =>
                {
                    var configuration = context.Resolve<IConfiguration>();
                    var options = configuration.GetOptions<SqlSettings>("sqlSettings");

                    return options;
                })
                .SingleInstance();

            builder.Register(context =>
                {
                    var sqlSettings = context.Resolve<SqlSettings>();
                    var dbContextOptions = new DbContextOptionsBuilder();

                    return new ProductServiceDbContext(sqlSettings, _loggerFactory, dbContextOptions.Options);
                })
                .AsSelf()
                .As<IUnitOfWork>();
        }
    }
}