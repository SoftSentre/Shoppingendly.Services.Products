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
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Entities
{
    public class Category : AuditableAndEventSourcingEntity<CategoryId>
    {
        private HashSet<ProductCategory> _productCategories = new HashSet<ProductCategory>();

        // Required for EF
        private Category()
        {
        }

        internal Category(CategoryId categoryId, string categoryName) : base(categoryId)
        {
            CategoryName = ValidateCategoryName(categoryName);
            CategoryIcon = Picture.Empty;
            AddDomainEvent(new NewCategoryCreatedDomainEvent(Id, categoryName, CategoryIcon));
        }

        internal Category(CategoryId categoryId, string categoryName, Picture categoryIcon) : base(categoryId)
        {
            CategoryName = ValidateCategoryName(categoryName);
            CategoryIcon = ValidateCategoryIcon(categoryIcon);
            AddDomainEvent(new NewCategoryCreatedDomainEvent(Id, categoryName, categoryIcon));
        }

        internal Category(CategoryId categoryId, string categoryName, string categoryDescription) : base(categoryId)
        {
            CategoryName = ValidateCategoryName(categoryName);
            CategoryDescription = ValidateCategoryDescription(categoryDescription);
            CategoryIcon = Picture.Empty;
            AddDomainEvent(new NewCategoryCreatedDomainEvent(categoryId, categoryName, categoryDescription, CategoryIcon));
        }

        internal Category(CategoryId categoryId, string categoryName, string categoryDescription,
            Picture categoryIcon) : base(categoryId)
        {
            CategoryName = ValidateCategoryName(categoryName);
            CategoryDescription = ValidateCategoryDescription(categoryDescription);
            CategoryIcon = ValidateCategoryIcon(categoryIcon);
            AddDomainEvent(
                new NewCategoryCreatedDomainEvent(categoryId, categoryName, categoryDescription, categoryIcon));
        }

        public string CategoryName { get; private set; }
        public string CategoryDescription { get; private set; }
        public Picture CategoryIcon { get; private set; }

        public HashSet<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set => _productCategories = new HashSet<ProductCategory>(value);
        }

        internal bool SetCategoryName(string categoryName)
        {
            ValidateCategoryName(categoryName);

            if (CategoryName.EqualsCaseInvariant(categoryName))
            {
                return false;
            }

            CategoryName = categoryName;
            SetUpdatedDate();
            AddDomainEvent(new CategoryNameChangedDomainEvent(Id, categoryName));
            return true;
        }

        internal bool SetCategoryDescription(string categoryDescription)
        {
            ValidateCategoryDescription(categoryDescription);

            if (CategoryDescription.EqualsCaseInvariant(categoryDescription))
            {
                return false;
            }

            CategoryDescription = categoryDescription;
            SetUpdatedDate();
            AddDomainEvent(new CategoryDescriptionChangedDomainEvent(Id, categoryDescription));
            return true;
        }

        internal bool AddOrChangeCategoryIcon(Maybe<Picture> categoryIcon)
        {
            var validateCategoryIcon = ValidateCategoryIcon(categoryIcon);

            if (!CategoryIcon.IsEmpty && CategoryIcon.Equals(validateCategoryIcon))
            {
                return false;
            }

            CategoryIcon = validateCategoryIcon;
            SetUpdatedDate();
            AddDomainEvent(new CategoryIconAddedOrChangedDomainEvent(Id, validateCategoryIcon));
            return true;
        }

        internal static Category Create(CategoryId categoryId, string categoryName)
        {
            return new Category(categoryId, categoryName);
        }

        internal static Category Create(CategoryId categoryId, string categoryName, string description)
        {
            return new Category(categoryId, categoryName, description);
        }

        private static string ValidateCategoryName(string categoryName)
        {
            if (IsCategoryNameRequired && categoryName.IsEmpty())
            {
                throw new CategoryNameCanNotBeEmptyException();
            }

            if (categoryName.IsLongerThan(CategoryNameMaxLength))
            {
                throw new CategoryNameIsTooLongException(CategoryNameMaxLength);
            }

            if (categoryName.IsShorterThan(CategoryNameMinLength))
            {
                throw new CategoryNameIsTooShortException(CategoryNameMinLength);
            }

            return categoryName;
        }

        private static string ValidateCategoryDescription(string categoryDescription)
        {
            if (IsCategoryDescriptionRequired && categoryDescription.IsEmpty())
            {
                throw new CategoryDescriptionCanNotBeEmptyException();
            }

            if (categoryDescription.IsShorterThan(CategoryDescriptionMinLength))
            {
                throw new CategoryDescriptionIsTooShortException(CategoryDescriptionMinLength);
            }

            if (categoryDescription.IsLongerThan(CategoryDescriptionMaxLength))
            {
                throw new CategoryDescriptionIsTooLongException(CategoryDescriptionMaxLength);
            }

            return categoryDescription;
        }

        private static Picture ValidateCategoryIcon(Maybe<Picture> categoryIcon)
        {
            if (categoryIcon.HasNoValue || categoryIcon.Value.IsEmpty)
            {
                throw new CategoryIconCanNotBeNullOrEmptyException();
            }

            return categoryIcon.Value;
        }
    }
}