namespace Shoppingendly.Services.Products.Core.Domain.Base.Identification
{
    public interface IIdentity<out TId>
    {
        TId Id { get; }
    }
}