using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects
{
    public class CategoryId : Identity<Guid>
    {
        public CategoryId()
        {
            Id = Guid.NewGuid();
        }

        public CategoryId(Guid value) : base(value)
        {
        }
    }
}