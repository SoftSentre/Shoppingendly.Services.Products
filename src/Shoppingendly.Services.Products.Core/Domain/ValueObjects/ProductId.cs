using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class ProductId : Identity<Guid>
    {
        public ProductId()
        {
        }

        public ProductId(Guid value) : base(value)
        {
        }
    }
}