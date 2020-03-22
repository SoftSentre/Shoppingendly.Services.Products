using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class CreatorId : Identity<Guid>
    {
        internal CreatorId()
        {
            Id = Guid.NewGuid();
        }

        internal CreatorId(Guid value) : base(value)
        {
        }
    }
}