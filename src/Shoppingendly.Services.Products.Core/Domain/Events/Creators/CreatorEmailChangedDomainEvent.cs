using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Creators
{
    public class CreatorEmailChangedDomainEvent : DomainEventBase
    {
        public CreatorId CreatorId { get; }
        public string Email { get; }

        internal CreatorEmailChangedDomainEvent(CreatorId creatorId, string email)
        {
            CreatorId = creatorId;
            Email = email;
        }
    }
}