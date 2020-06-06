// Copyright 2020 SoftSentre Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Core.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Core.Domain.Entities
{
    public class Creator : AuditableAndEventSourcingEntity<CreatorId>
    {
        private HashSet<Product> _products = new HashSet<Product>();

        // Required for EF
        private Creator()
        {
        }

        internal Creator(CreatorId creatorId, string name, Role role) : base(creatorId)
        {
            Name = ValidateName(name);
            Role = ValidateRole(role);
            AddDomainEvent(new NewCreatorCreatedDomainEvent(creatorId, name, role));
        }

        public int RoleId { get; private set; }
        public string Name { get; private set; }

        // Navigation property
        public Role Role { get; private set; }

        public HashSet<Product> Products
        {
            get => _products;
            set => _products = new HashSet<Product>(value);
        }

        internal void SetName(string name)
        {
            ValidateName(name);

            Name = name;
            SetUpdatedDate();
            AddDomainEvent(new CreatorNameChangedDomainEvent(Id, name));
        }
        
        internal void SetRole(Role role)
        {
            ValidateRole(role);

            Role = role;
            SetUpdatedDate();
            AddDomainEvent(new CreatorRoleChangedDomainEvent(Id, role));
        }

        internal static Creator Create(CreatorId creatorId, string name, Role role)
        {
            return new Creator(creatorId, name, role);
        }

        private static string ValidateName(string name)
        {
            if (IsCreatorNameRequired && name.IsEmpty())
            {
                throw new InvalidCreatorNameException("Creator name can not be empty.");
            }

            if (name.IsLongerThan(CreatorNameMaxLength))
            {
                throw new InvalidCreatorNameException(
                    $"Creator name can not be longer than {CreatorNameMaxLength} characters.");
            }

            if (name.IsShorterThan(CreatorNameMinLength))
            {
                throw new InvalidCreatorNameException(
                    $"Creator name can not be shorter than {CreatorNameMinLength} characters.");
            }

            return name;
        }


        private static Role ValidateRole(Role role)
        {
            if (role.Name.IsLongerThan(RoleNameMaxLength))
            {
                throw new ArgumentException(
                    $"Creator role name can not be longer than {CreatorNameMinLength} characters.");
            }

            return role;
        }
    }
}