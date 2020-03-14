using System.Collections.Generic;
using System.Text.RegularExpressions;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Creators;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Creators;
using Shoppingendly.Services.Products.Core.Extensions;
using static Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace Shoppingendly.Services.Products.Core.Domain.Entities
{
    public class Creator : AuditableAndEventSourcingEntity<CreatorId>
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private HashSet<Product> _products;
        
        public int RoleId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        // Navigation property
        public Role Role { get; private set; }

        public HashSet<Product> Products
        {
            get => _products;
            set => _products = new HashSet<Product>(value);
        }

        public Creator()
        {
            // Required for EF
        }

        public Creator(CreatorId creatorId, string name, string email, Role role) : base(creatorId)
        {
            Name = ValidateName(name);
            Email = ValidateEmail(email);
            Role = ValidateRole(role);
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
            ValidateRole(role);

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
            if (IsCreatorNameRequired && name.IsEmpty())
                throw new InvalidCreatorNameException("Creator name can not be empty.");
            if (name.IsLongerThan(CreatorNameMaxLength))
                throw new InvalidCreatorNameException(
                    $"Creator name can not be longer than {CreatorNameMaxLength} characters.");
            if (name.IsShorterThan(CreatorNameMinLength))
                throw new InvalidCreatorNameException(
                    $"Creator name can not be shorter than {CreatorNameMinLength} characters.");

            return name;
        }

        private static string ValidateEmail(string email)
        {
            if (IsCreatorEmailRequired && email.IsEmpty())
                throw new InvalidCreatorEmailException("Creator email can not be empty.");
            if (email.IsShorterThan(CreatorEmailMinLength))
                throw new InvalidCreatorEmailException(
                    $"Creator email can not be shorter than {CreatorEmailMinLength} characters.");
            if (email.IsLongerThan(CreatorEmailMaxLength))
                throw new InvalidCreatorEmailException(
                    $"Creator email can not be longer than {CreatorEmailMaxLength} characters.");
            if (!EmailRegex.IsMatch(email))
                throw new InvalidCreatorEmailException("Invalid email has been provided.");

            return email;
        }

        private static Role ValidateRole(Role role)
        {
            if (role.Name.IsLongerThan(RoleNameMaxLength))
                throw new InvalidCreatorRoleException(
                    $"Creator role name can not be longer than {CreatorNameMinLength} characters.");

            return role;
        }
    }
}