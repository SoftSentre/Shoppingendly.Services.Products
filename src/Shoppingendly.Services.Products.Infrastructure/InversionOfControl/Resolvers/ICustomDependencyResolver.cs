namespace Shoppingendly.Services.Products.Infrastructure.InversionOfControl.Resolvers
{
    public interface ICustomDependencyResolver
    {
        TDependency Resolve<TDependency>();
    }
}