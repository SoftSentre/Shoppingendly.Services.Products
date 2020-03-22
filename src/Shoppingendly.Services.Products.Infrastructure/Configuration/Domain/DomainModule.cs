using Autofac;
using Shoppingendly.Services.Products.Core.Domain.Services;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;

namespace Shoppingendly.Services.Products.Infrastructure.Configuration.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductDomainService>()
                .As<IProductDomainService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<CategoryDomainService>()
                .As<ICategoryDomainService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<CreatorDomainService>()
                .As<ICreatorDomainService>()
                .InstancePerLifetimeScope();
        }
    }
}