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
using System.Linq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates
{
    public class Product : EventSourcingEntity, IAggregateRoot
    {
        private HashSet<ProductCategory> _productCategories = new HashSet<ProductCategory>();

        // Required for EF
        private Product()
        {
        }

        internal Product(ProductId productId, CreatorId creatorId, string productName, ProductProducer productProducer)
        {
            ProductId = productId;
            CreatorId = creatorId;
            ProductPicture = Picture.Empty;
            ProductName = productName;
            ProductProducer = productProducer;
        }

        internal Product(ProductId productId, CreatorId creatorId, Picture productPicture, string productName,
            ProductProducer productProducer)
        {
            ProductId = productId;
            CreatorId = creatorId;
            ProductPicture = productPicture;
            ProductName = productName;
            ProductProducer = productProducer;
        }

        public ProductId ProductId { get; }
        public CreatorId CreatorId { get; }
        public Picture ProductPicture { get; private set; }
        public string ProductName { get; private set; }
        public ProductProducer ProductProducer { get; private set; }

        // Navigation property
        public Creator Creator { get; set; }

        public HashSet<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set => _productCategories = new HashSet<ProductCategory>(value);
        }

        internal bool ChangeProductName(string productName)
        {
            if (ProductName.EqualsCaseInvariant(productName))
            {
                return false;
            }

            ProductName = productName;
            SetUpdatedDate();
            return true;
        }

        internal bool ChangeProductProducer(ProductProducer productProducer)
        {
            if (ProductProducer.Name.EqualsCaseInvariant(productProducer.Name))
            {
                return false;
            }

            ProductProducer = productProducer;
            SetUpdatedDate();
            return true;
        }

        internal bool UploadProductPicture(Picture productPicture)
        {
            if (ProductPicture.Equals(productPicture))
            {
                return false;
            }

            ProductPicture = productPicture;
            SetUpdatedDate();
            return true;
        }

        internal void AssignCategory(CategoryId categoryId)
        {
            var assignedCategory = GetProductCategory(categoryId);

            if (assignedCategory.HasValue)
            {
                throw new ProductIsAlreadyAssignedToCategoryException(categoryId);
            }

            var newAssignedCategory = new ProductCategory(ProductId, categoryId);
            _productCategories.Add(newAssignedCategory);
            SetUpdatedDate();
        }

        internal void DeallocateCategory(CategoryId categoryId)
        {
            var assignedCategory = GetProductCategory(categoryId);

            if (assignedCategory.HasNoValue)
            {
                throw new ProductWithAssignedCategoryNotFoundException(categoryId);
            }

            _productCategories.Remove(assignedCategory.Value);
            SetUpdatedDate();
        }

        internal void DeallocateAllCategories()
        {
            if (!_productCategories.Any())
            {
                throw new ProductWithAssignedCategoriesNotFoundException();
            }

            _productCategories.Clear();
            SetUpdatedDate();
        }

        internal Maybe<IEnumerable<ProductCategory>> GetAllAssignedCategories()
        {
            return _productCategories;
        }

        private Maybe<ProductCategory> GetProductCategory(CategoryId categoryId)
        {
            var assignedCategory = _productCategories.FirstOrDefault(pr => pr.CategoryId.Equals(categoryId));

            return assignedCategory;
        }
    }
}