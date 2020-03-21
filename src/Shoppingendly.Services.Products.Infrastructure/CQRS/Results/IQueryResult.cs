namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Results
{
    public interface IQueryResult<out T> : IResult
    {
        T Data { get; }
    }
}