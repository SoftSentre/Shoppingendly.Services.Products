using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class CategoryId : Identity<Guid>
    {
        internal CategoryId()
        {
            Id = Guid.NewGuid();
        }

        internal CategoryId(Guid value) : base(value)
        {
        }
    }
}