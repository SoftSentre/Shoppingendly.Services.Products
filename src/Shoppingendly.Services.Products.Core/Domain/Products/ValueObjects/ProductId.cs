using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects
{
    public class ProductId : Identity
    {
        public ProductId()
        {
        }

        public ProductId(Guid value) : base(value)
        {
        }
    }
}