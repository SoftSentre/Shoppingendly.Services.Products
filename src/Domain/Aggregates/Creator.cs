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
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates
{
    public class Creator : EventSourcingEntity, IAggregateRoot
    {
        private HashSet<Product> _products = new HashSet<Product>();

        // Required for EF
        private Creator()
        {
        }

        internal Creator(CreatorId creatorId, string creatorName, CreatorRole creatorRole)
        {
            CreatorId = creatorId;
            CreatorName = creatorName;
            CreatorRole = creatorRole;
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

        internal void ChangeCreatorName(string creatorName)
        {
            CreatorName = creatorName;
            SetUpdatedDate();
        }

        internal void ChangeCreatorRole(CreatorRole creatorRole)
        {
            CreatorRole = creatorRole;
            SetUpdatedDate();
        }
    }
}