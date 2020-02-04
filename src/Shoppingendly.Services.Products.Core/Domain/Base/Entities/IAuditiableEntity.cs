using System;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public interface IAuditAbleEntity
    {
        DateTime UpdatedDate { get; }
        DateTime CreatedAt { get; }
    }
}