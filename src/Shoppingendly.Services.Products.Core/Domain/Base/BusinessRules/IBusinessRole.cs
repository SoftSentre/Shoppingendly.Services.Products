namespace Shoppingendly.Services.Products.Core.Domain.Base.BusinessRules
{
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}