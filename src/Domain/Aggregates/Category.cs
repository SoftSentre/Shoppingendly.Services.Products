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
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates
{
    public class Category : EventSourcingEntity, IAggregateRoot
    {
        private HashSet<Categorization> _assignedProducts = new HashSet<Categorization>();

        // Required for EF
        private Category()
        {
        }

        internal Category(CategoryId categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryIcon = Picture.Empty;
        }

        internal Category(CategoryId categoryId, CategoryId parentCategoryId, string categoryName)
        {
            CategoryId = categoryId;
            ParentCategoryId = parentCategoryId;
            CategoryName = categoryName;
            CategoryIcon = Picture.Empty;
        }

        internal Category(CategoryId categoryId, string categoryName, Picture categoryIcon)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryIcon = categoryIcon;
        }

        internal Category(CategoryId categoryId, CategoryId parentCategoryId, string categoryName, Picture categoryIcon)
        {
            CategoryId = categoryId;
            ParentCategoryId = parentCategoryId;
            CategoryName = categoryName;
            CategoryIcon = categoryIcon;
        }

        internal Category(CategoryId categoryId, string categoryName, string categoryDescription)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            CategoryIcon = Picture.Empty;
        }

        internal Category(CategoryId categoryId, CategoryId parentCategoryId, string categoryName,
            string categoryDescription)
        {
            CategoryId = categoryId;
            ParentCategoryId = parentCategoryId;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            CategoryIcon = Picture.Empty;
        }

        internal Category(CategoryId categoryId, string categoryName, string categoryDescription,
            Picture categoryIcon)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            CategoryIcon = categoryIcon;
        }

        internal Category(CategoryId categoryId, CategoryId parentCategoryId, string categoryName,
            string categoryDescription,
            Picture categoryIcon)
        {
            CategoryId = categoryId;
            ParentCategoryId = parentCategoryId;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            CategoryIcon = categoryIcon;
        }

        public CategoryId CategoryId { get; }
        public CategoryId ParentCategoryId { get; }
        public string CategoryName { get; private set; }
        public string CategoryDescription { get; private set; }
        public Picture CategoryIcon { get; private set; }

        public HashSet<Categorization> AssignedProducts
        {
            get => _assignedProducts;
            set => _assignedProducts = new HashSet<Categorization>(value);
        }

        internal bool ChangeCategoryName(string categoryName)
        {
            if (CategoryName.EqualsCaseInvariant(categoryName))
            {
                return false;
            }

            CategoryName = categoryName;
            SetUpdatedDate();
            return true;
        }

        internal bool SetCategoryDescription(string categoryDescription)
        {
            if (CategoryDescription.EqualsCaseInvariant(categoryDescription))
            {
                return false;
            }

            CategoryDescription = categoryDescription;
            SetUpdatedDate();
            return true;
        }

        internal bool UploadCategoryIcon(Picture categoryIcon)
        {
            if (!CategoryIcon.IsEmpty && CategoryIcon.Equals(categoryIcon))
            {
                return false;
            }

            CategoryIcon = categoryIcon;
            SetUpdatedDate();
            return true;
        }
    }
}