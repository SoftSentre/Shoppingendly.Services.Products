namespace Shoppingendly.Services.Products.Core.Types
{
    public interface IIdentifiable<out TId>
    {
        TId Id { get; }
    }
}