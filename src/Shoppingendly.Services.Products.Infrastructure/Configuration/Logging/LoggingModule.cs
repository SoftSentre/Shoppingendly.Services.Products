using Autofac;
using Microsoft.Extensions.Configuration;
using Shoppingendly.Services.Products.Infrastructure.Extensions;
using Shoppingendly.Services.Products.Infrastructure.Logger;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.Logging
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var configuration = context.Resolve<IConfiguration>();
                    var options = configuration.GetOptions<LoggerSettings>("loggerSettings");

                    return options;
                })
                .SingleInstance();
        }
    }
}