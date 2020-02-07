using System;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Products.Events.Creators;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Entities
{
    public class Creator : AuditableAndEventSourcingEntity<CreatorId>
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public Role Role { get; private set; }

        public Creator()
        {
            // Required for EF
        }

        public Creator(CreatorId creatorId, string name, string email, Role role) : base(creatorId)
        {
            Name = name;
            Email = email;
            Role = role;
            AddDomainEvent(new NewCreatorCreatedDomainEvent(creatorId, name, email, role));
        }

        public bool SetName(string name)
        {
            AddDomainEvent(new CreatorNameChangedDomainEvent(Id, name));
            return true;
        }

        public void SetEmail(string email)
        {
            
        }

        public void SetRole(Role role)
        {
            
        }

        public static Creator Create(CreatorId creatorId, string name, string email, Role role)
        {
            return new Creator(creatorId, name, email, role);
        }
        
        private string ValidateName(string name)
        {
            return string.Empty;
        }

        private string ValidateEmail(string email)
        {
            return string.Empty;
        }

        private string ValidateRole(Role role)
        {
            return string.Empty;
        }
    }
}