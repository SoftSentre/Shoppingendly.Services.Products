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
            using var scope = _lifetimeScope.BeginLifetimeScope();
            var resolvedDependency = scope.Resolve<TDependency>();
            return resolvedDependency;
        }
    }
}