namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    public class ProductIsAlreadyAssignedToCategoryException : ShoppingendlyException
    {
        public ProductIsAlreadyAssignedToCategoryException(string message, params object[] args) : base(message, args)
        {
        }
    }
}