using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects
{
    public class CategoryId : Identity
    {
        public CategoryId()
        {
        }

        public CategoryId(Guid value) : base(value)
        {
        }
    }
}