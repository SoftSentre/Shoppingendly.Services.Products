namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class ProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        public ProductWithAssignedCategoryNotFoundException(string message) : base(message)
        {
        }
    }
}