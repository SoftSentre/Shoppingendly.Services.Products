using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Creators
{
    public class NewCreatorCreatedDomainEvent : DomainEventBase
    {
        public CreatorId CreatorId { get; }
        public string Name { get; }
        public string Email { get; }
        public Role Role { get; }

        internal NewCreatorCreatedDomainEvent()
        {
            // only for blocking creation of new object in other assembly than this.
        }
        
        internal NewCreatorCreatedDomainEvent(CreatorId creatorId, string name, string email, Role role)
        {
            CreatorId = creatorId;
            Name = name;
            Email = email;
            Role = role;
        }
    }
}