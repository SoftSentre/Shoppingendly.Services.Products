namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public abstract class Entity<TId> : EntityBase<TId>
    {
        protected Entity(TId id) : base(id)
        {
        }
    }
}