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
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Core.Exceptions.Categories;
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Core.Domain.Entities
{
    public class Category : AuditableAndEventSourcingEntity<CategoryId>
    {
        private HashSet<ProductCategory> _productCategories = new HashSet<ProductCategory>();

        // Required for EF
        private Category()
        {
        }

        internal Category(CategoryId categoryId, string name) : base(categoryId)
        {
            Name = ValidateCategoryName(name);
            AddDomainEvent(new NewCategoryCreatedDomainEvent(Id, name));
        }

        internal Category(CategoryId categoryId, string name, string description) : base(categoryId)
        {
            Name = ValidateCategoryName(name);
            Description = ValidateCategoryDescription(description);
            AddDomainEvent(new NewCategoryCreatedDomainEvent(categoryId, name, description));
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public HashSet<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set => _productCategories = new HashSet<ProductCategory>(value);
        }

        internal bool SetName(string name)
        {
            ValidateCategoryName(name);

            if (Name.EqualsCaseInvariant(name))
            {
                return false;
            }

            Name = name;
            SetUpdatedDate();
            AddDomainEvent(new CategoryNameChangedDomainEvent(Id, name));
            return true;
        }

        internal bool SetDescription(string description)
        {
            ValidateCategoryDescription(description);

            if (Description.EqualsCaseInvariant(description))
            {
                return false;
            }

            Description = description;
            SetUpdatedDate();
            AddDomainEvent(new CategoryDescriptionChangedDomainEvent(Id, description));
            return true;
        }

        internal static Category Create(CategoryId categoryId, string name)
        {
            return new Category(categoryId, name);
        }

        internal static Category Create(CategoryId categoryId, string name, string description)
        {
            return new Category(categoryId, name, description);
        }

        private static string ValidateCategoryName(string name)
        {
            if (IsCategoryNameRequired && name.IsEmpty())
            {
                throw new InvalidCategoryNameException("Category name can not be empty.");
            }

            if (name.IsLongerThan(CategoryNameMaxLength))
            {
                throw new InvalidCategoryNameException(
                    $"Category name can not be longer than {CategoryNameMaxLength} characters.");
            }

            if (name.IsShorterThan(CategoryNameMinLength))
            {
                throw new InvalidCategoryNameException(
                    $"Category name can not be shorter than {CategoryNameMinLength} characters.");
            }

            return name;
        }

        private static string ValidateCategoryDescription(string description)
        {
            if (IsCategoryDescriptionRequired && description.IsEmpty())
            {
                throw new InvalidCategoryDescriptionException("Category description can not be empty.");
            }

            if (description.IsShorterThan(CategoryDescriptionMinLength))
            {
                throw new InvalidCategoryDescriptionException(
                    $"Category description can not be shorter than {CategoryDescriptionMinLength} characters.");
            }

            if (description.IsLongerThan(CategoryDescriptionMaxLength))
            {
                throw new InvalidCategoryDescriptionException(
                    $"Category description can not be longer than {CategoryDescriptionMaxLength} characters.");
            }

            return description;
        }
    }
}