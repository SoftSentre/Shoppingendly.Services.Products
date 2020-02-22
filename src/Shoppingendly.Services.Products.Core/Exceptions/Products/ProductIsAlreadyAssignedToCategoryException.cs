namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class ProductIsAlreadyAssignedToCategoryException : ShoppingendlyException
    {
        public ProductIsAlreadyAssignedToCategoryException(string message) : base(message)
        {
        }
    }
}