namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    internal class ProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        internal ProductWithAssignedCategoryNotFoundException(string message) : base(message)
        {
        }
    }
}