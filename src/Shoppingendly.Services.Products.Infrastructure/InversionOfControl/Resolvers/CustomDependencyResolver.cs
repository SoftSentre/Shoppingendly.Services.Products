using Autofac;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Infrastructure.InversionOfControl.Resolvers
{
    public class CustomDependencyResolver : ICustomDependencyResolver
    {
        private readonly ILifetimeScope _lifetimeScope;

        public CustomDependencyResolver(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope
                .IfEmptyThenThrowAndReturnValue();
        }

        public TDependency Resolve<TDependency>()
        {
            var resolvedDependency = _lifetimeScope.Resolve<TDependency>();
            return resolvedDependency;
        }
    }
}