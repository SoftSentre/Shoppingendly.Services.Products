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

using System.Collections.Generic;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Entities
{
    public class Creator : EventSourcingEntity
    {
        private HashSet<Product> _products = new HashSet<Product>();

        // Required for EF
        private Creator()
        {
        }

        internal Creator(CreatorId creatorId, string creatorName, CreatorRole creatorRole)
        {
            CreatorId = creatorId;
            CreatorName = ValidateName(creatorName);
            CreatorRole = ValidateRole(creatorRole);
            AddDomainEvent(new NewCreatorCreatedDomainEvent(creatorId, creatorName, creatorRole));
        }

        public CreatorId CreatorId { get; }
        public int CreatorRoleId { get; private set; }
        public string CreatorName { get; private set; }

        // Navigation property
        public CreatorRole CreatorRole { get; private set; }

        public HashSet<Product> Products
        {
            get => _products;
            set => _products = new HashSet<Product>(value);
        }

        internal void SetCreatorName(string creatorName)
        {
            ValidateName(creatorName);

            CreatorName = creatorName;
            SetUpdatedDate();
            AddDomainEvent(new CreatorNameChangedDomainEvent(CreatorId, creatorName));
        }

        internal void SetCreatorRole(CreatorRole creatorRole)
        {
            ValidateRole(creatorRole);

            CreatorRole = creatorRole;
            SetUpdatedDate();
            AddDomainEvent(new CreatorRoleChangedDomainEvent(CreatorId, creatorRole));
        }

        internal static Creator Create(CreatorId creatorId, string creatorName, CreatorRole creatorRole)
        {
            return new Creator(creatorId, creatorName, creatorRole);
        }

        private static string ValidateName(string creatorName)
        {
            if (IsCreatorNameRequired && creatorName.IsEmpty())
            {
                throw new CreatorNameCanNotBeEmptyException();
            }

            if (creatorName.IsLongerThan(CreatorNameMaxLength))
            {
                throw new CreatorNameIsTooLongException(CreatorNameMaxLength);
            }

            if (creatorName.IsShorterThan(CreatorNameMinLength))
            {
                throw new CreatorNameIsTooShortException(CreatorNameMinLength);
            }

            return creatorName;
        }

        private static CreatorRole ValidateRole(CreatorRole creatorRole)
        {
            if (creatorRole.Name.IsLongerThan(CreatorRoleNameMaxLength))
            {
                throw new RoleIsTooLongException(CreatorRoleNameMaxLength);
            }

            return creatorRole;
        }
    }
}