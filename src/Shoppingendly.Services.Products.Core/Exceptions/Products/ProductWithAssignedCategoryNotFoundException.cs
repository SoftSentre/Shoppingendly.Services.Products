namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class ProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        public ProductWithAssignedCategoryNotFoundException(string message, params object[] args) : base(message, args)
        {
        }
    }
}