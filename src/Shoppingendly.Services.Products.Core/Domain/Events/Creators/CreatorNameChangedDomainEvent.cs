using Shoppingendly.Services.Products.Core.Domain.Base.DomainEvents;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Events.Creators
{
    public class CreatorNameChangedDomainEvent : DomainEventBase
    {
        public CreatorId CreatorId { get; }
        public string Name { get; }
        
        public CreatorNameChangedDomainEvent(CreatorId creatorId, string name)
        {
            CreatorId = creatorId;
            Name = name;
        }
    }
}