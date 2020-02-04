using System;

namespace Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccuredAt { get; }
    }
}