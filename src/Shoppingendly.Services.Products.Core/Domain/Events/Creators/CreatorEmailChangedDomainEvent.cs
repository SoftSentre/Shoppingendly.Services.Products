using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Creators
{
    public class CreatorEmailChangedDomainEvent : DomainEventBase
    {
        public CreatorId CreatorId { get; }
        public string Email { get; }

        internal CreatorEmailChangedDomainEvent()
        {
            // only for blocking creation of new object in other assembly than this.
        }
        
        internal CreatorEmailChangedDomainEvent(CreatorId creatorId, string email)
        {
            CreatorId = creatorId;
            Email = email;
        }
    }
}