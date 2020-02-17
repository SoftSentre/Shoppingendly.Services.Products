namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class AnyProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        public AnyProductWithAssignedCategoryNotFoundException(string message, params object[] args) : base(message,
            args)
        {
        }
    }
}