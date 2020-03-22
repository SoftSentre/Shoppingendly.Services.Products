namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    internal class AnyProductWithAssignedCategoryNotFoundException : ShoppingendlyException
    {
        internal AnyProductWithAssignedCategoryNotFoundException(string message) : base(message)
        {
        }
    }
}