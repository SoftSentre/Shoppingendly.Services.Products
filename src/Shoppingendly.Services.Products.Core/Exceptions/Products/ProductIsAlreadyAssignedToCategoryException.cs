namespace Shoppingendly.Services.Products.Core.Exceptions.Products
{
    internal class ProductIsAlreadyAssignedToCategoryException : ShoppingendlyException
    {
        internal ProductIsAlreadyAssignedToCategoryException(string message) : base(message)
        {
        }
    }
}