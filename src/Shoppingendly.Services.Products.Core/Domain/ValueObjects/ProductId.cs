using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class ProductId : Identity<Guid>
    {
        internal ProductId()
        {
            Id = Guid.NewGuid();
        }

        internal ProductId(Guid value) : base(value)
        {
        }
    }
}