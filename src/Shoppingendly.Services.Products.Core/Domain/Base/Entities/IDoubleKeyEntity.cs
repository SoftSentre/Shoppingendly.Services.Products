namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public interface IDoubleKeyEntity<out TFirstId, out TSecondId>
    {
        TFirstId FirstKey { get; }
        TSecondId SecondKey { get; }
    }
}