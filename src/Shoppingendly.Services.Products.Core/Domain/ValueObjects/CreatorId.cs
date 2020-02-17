using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class CreatorId : Identity<Guid>
    {
        public CreatorId()
        {
            Id = Guid.NewGuid();
        }

        public CreatorId(Guid value) : base(value)
        {
        }
    }
}