using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Events.Creators
{
    public class CreatorEmailChangedDomainEvent : DomainEventBase
    {
        public CreatorId CreatorId { get; }
        public string Email { get; }

        public CreatorEmailChangedDomainEvent(CreatorId creatorId, string email)
        {
            CreatorId = creatorId;
            Email = email;
        }
    }
}