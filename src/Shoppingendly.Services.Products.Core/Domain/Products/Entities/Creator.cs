using System.Text.RegularExpressions;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Products.Events.Creators;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Creators;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Entities
{
    public class Creator : AuditableAndEventSourcingEntity<CreatorId>
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Name { get; private set; }
        public string Email { get; private set; }
        public Role Role { get; private set; }

        public Creator()
        {
            // Required for EF
        }

        public Creator(CreatorId creatorId, string name, string email, Role role) : base(creatorId)
        {
            Name = ValidateName(name);
            Email = ValidateEmail(email);
            Role = role;
            AddDomainEvent(new NewCreatorCreatedDomainEvent(creatorId, name, email, role));
        }

        public void SetName(string name)
        {
            ValidateName(name);

            Name = name;
            SetUpdatedDate();
            AddDomainEvent(new CreatorNameChangedDomainEvent(Id, name));
        }

        public void SetEmail(string email)
        {
            ValidateEmail(email);

            Email = email;
            SetUpdatedDate();
            AddDomainEvent(new CreatorEmailChangedDomainEvent(Id, email));
        }

        public void SetRole(Role role)
        {
            Role = role;
            SetUpdatedDate();
            AddDomainEvent(new CreatorRoleChangedDomainEvent(Id, role));
        }

        public static Creator Create(CreatorId creatorId, string name, string email, Role role)
        {
            return new Creator(creatorId, name, email, role);
        }

        private static string ValidateName(string name)
        {
            if (name.IsEmpty())
                throw new InvalidCreatorNameException("Creator name can not be empty.", name);
            if (name.IsLongerThan(50))
                throw new InvalidCreatorNameException("Creator name can not be longer than 50 characters.", name);
            if (name.IsShorterThan(3))
                throw new InvalidCreatorNameException("Creator name can not be shorter than 3 characters.", name);

            return name;
        }

        private static string ValidateEmail(string email)
        {
            if (email.IsEmpty())
                throw new InvalidCreatorEmailException("Creator email can not be empty.", email);
            if (!EmailRegex.IsMatch(email))
                throw new InvalidCreatorEmailException("Invalid email has been provided.", email);

            return email;
        }
    }
}