namespace Shoppingendly.Services.Products.Application.CQRS.Base.Results
{
    public interface IQueryResult<out T> : IResult
    {
        T Data { get; }
    }
}