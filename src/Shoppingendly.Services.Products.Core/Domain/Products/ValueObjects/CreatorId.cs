using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects
{
    public class CreatorId : Identity
    {
        public CreatorId()
        {
        }

        public CreatorId(Guid value) : base(value)
        {
        }
    }
}