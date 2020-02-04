namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public interface IEntity<out TId> 
    {
        TId Id { get; }
    }
}