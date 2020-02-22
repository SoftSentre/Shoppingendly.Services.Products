namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class AnyProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        public AnyProductWithAssignedCategoryNotFoundException(string message) : base(message)
        {
        }
    }
}