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
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Pictures;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Producers;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates
{
    public class Product : AuditableAndEventSourcingEntity<ProductId>, IAggregateRoot
    {
        private HashSet<ProductCategory> _productCategories = new HashSet<ProductCategory>();

        // Required for EF
        private Product()
        {
        }

        internal Product(ProductId id, CreatorId creatorId, string productName, ProductProducer producer) : base(id)
        {
            CreatorId = creatorId;
            ProductPicture = ProductPicture.Empty;
            ProductName = ValidateProductName(productName);
            Producer = producer;
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, productName, producer,
                ProductPicture.Empty));
        }

        internal Product(ProductId id, CreatorId creatorId, ProductPicture productPicture, string productName,
            ProductProducer producer) : base(id)
        {
            CreatorId = creatorId;
            ProductPicture = productPicture;
            ProductName = ValidateProductName(productName);
            Producer = producer;
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, productName, producer, productPicture));
        }

        public CreatorId CreatorId { get; }
        public ProductPicture ProductPicture { get; private set; }
        public string ProductName { get; private set; }
        public ProductProducer Producer { get; private set; }

        // Navigation property
        public Creator Creator { get; set; }

        public HashSet<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set => _productCategories = new HashSet<ProductCategory>(value);
        }

        internal bool SetProductName(string productName)
        {
            ValidateProductName(productName);

            if (ProductName.EqualsCaseInvariant(productName))
            {
                return false;
            }

            ProductName = productName;
            SetUpdatedDate();
            AddDomainEvent(new ProductNameChangedDomainEvent(Id, productName));
            return true;
        }

        internal bool SetProductProducer(ProductProducer productProducer)
        {
            if (productProducer == null)
            {
                throw new ProductProducerCanNotBeNullException();
            }

            if (Producer.Name.EqualsCaseInvariant(productProducer.Name))
            {
                return false;
            }

            Producer = productProducer;
            SetUpdatedDate();
            AddDomainEvent(new ProductProducerChangedDomainEvent(Id, productProducer));
            return true;
        }

        internal bool AddOrChangeProductPicture(Maybe<ProductPicture> productPicture)
        {
            var validatedProductPicture = ValidatePicture(productPicture);

            if (!ProductPicture.IsEmpty && ProductPicture.Equals(validatedProductPicture))
            {
                return false;
            }

            ProductPicture = validatedProductPicture;
            SetUpdatedDate();
            AddDomainEvent(new PictureAddedOrChangedDomainEvent(Id, validatedProductPicture));
            return true;
        }

        internal void RemoveProductPicture()
        {
            if (ProductPicture.IsEmpty)
            {
                throw new PictureCanNotBeNullOrEmptyException();
            }

            ProductPicture = ProductPicture.Empty;
            SetUpdatedDate();
            AddDomainEvent(new PictureRemovedDomainEvent(Id));
        }

        internal void AssignCategory(CategoryId categoryId)
        {
            var assignedCategory = GetProductCategory(categoryId);

            if (assignedCategory.HasValue)
            {
                throw new ProductIsAlreadyAssignedToCategoryException(categoryId);
            }

            var newAssignedCategory = new ProductCategory(Id, categoryId);
            _productCategories.Add(newAssignedCategory);
            SetUpdatedDate();
            AddDomainEvent(new ProductAssignedToCategoryDomainEvent(newAssignedCategory.FirstKey,
                newAssignedCategory.SecondKey));
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
            AddDomainEvent(new ProductDeallocatedFromCategoryDomainEvent(assignedCategory.Value.FirstKey,
                assignedCategory.Value.SecondKey));
        }

        internal void DeallocateAllCategories()
        {
            if (!_productCategories.Any())
            {
                throw new ProductWithAssignedCategoriesNotFoundException();
            }

            var categoriesIds = _productCategories.Select(pc => pc.SecondKey).ToList();
            _productCategories.Clear();
            SetUpdatedDate();
            AddDomainEvent(new ProductDeallocatedFromAllCategoriesDomainEvent(Id, categoriesIds));
        }

        internal Maybe<IEnumerable<ProductCategory>> GetAllAssignedCategories()
        {
            return _productCategories;
        }

        internal Maybe<ProductCategory> GetAssignedCategory(CategoryId categoryId)
        {
            return GetProductCategory(categoryId);
        }

        internal static Product Create(ProductId id, CreatorId creatorId, string productName, ProductProducer producer)
        {
            return new Product(id, creatorId, productName, producer);
        }

        private static string ValidateProductName(string productName)
        {
            if (IsProductNameRequired && productName.IsEmpty())
            {
                throw new ProductNameCanNotBeEmptyException();
            }

            if (productName.IsLongerThan(ProductNameMaxLength))
            {
                throw new ProductNameIsTooLongException(ProductNameMaxLength);
            }

            if (productName.IsShorterThan(ProductNameMinLength))
            {
                throw new ProductNameIsTooShortException(ProductNameMinLength);
            }

            return productName;
        }

        private static ProductPicture ValidatePicture(Maybe<ProductPicture> productPicture)
        {
            if (productPicture.HasNoValue || productPicture.Value.IsEmpty)
            {
                throw new PictureCanNotBeNullOrEmptyException();
            }

            return productPicture.Value;
        }

        private Maybe<ProductCategory> GetProductCategory(CategoryId categoryId)
        {
            var assignedCategory = _productCategories.FirstOrDefault(pr => pr.SecondKey.Equals(categoryId));

            return assignedCategory;
        }
    }
}