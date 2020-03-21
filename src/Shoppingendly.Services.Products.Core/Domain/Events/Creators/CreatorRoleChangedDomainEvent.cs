using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Creators
{
    public class CreatorRoleChangedDomainEvent : DomainEventBase
    {
        public CreatorId CreatorId { get; }
        public Role Role { get; }

        internal CreatorRoleChangedDomainEvent()
        {
            // only for blocking creation of new object in other assembly than this.
        }
        
        internal CreatorRoleChangedDomainEvent(CreatorId creatorId, Role role)
        {
            CreatorId = creatorId;
            Role = role;
        }
    }
}