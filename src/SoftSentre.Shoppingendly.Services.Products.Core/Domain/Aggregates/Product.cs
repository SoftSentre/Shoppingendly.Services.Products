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
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Events.Products;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Core.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Core.Types;
using static SoftSentre.Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Core.Domain.Aggregates
{
    public class Product : AuditableAndEventSourcingEntity<ProductId>, IAggregateRoot
    {
        private HashSet<ProductCategory> _productCategories = new HashSet<ProductCategory>();

        // Required for EF
        private Product()
        {
        }

        internal Product(ProductId id, CreatorId creatorId, string name, ProductProducer producer) : base(id)
        {
            CreatorId = creatorId;
            Picture = Picture.Empty;
            Name = ValidateName(name);
            Producer = producer;
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, name, producer, Picture.Empty));
        }

        internal Product(ProductId id, CreatorId creatorId, Picture picture, string name, ProductProducer producer) : base(id)
        {
            CreatorId = creatorId;
            Picture = picture;
            Name = ValidateName(name);
            Producer = producer;
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, name, producer, picture));
        }

        public CreatorId CreatorId { get; }
        public Picture Picture { get; private set; }
        public string Name { get; private set; }
        public ProductProducer Producer { get; private set; }

        // Navigation property
        public Creator Creator { get; set; }

        public HashSet<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set => _productCategories = new HashSet<ProductCategory>(value);
        }

        internal bool SetName(string name)
        {
            ValidateName(name);

            if (Name.EqualsCaseInvariant(name))
            {
                return false;
            }

            Name = name;
            SetUpdatedDate();
            AddDomainEvent(new ProductNameChangedDomainEvent(Id, name));
            return true;
        }

        internal bool SetProducer(ProductProducer producer)
        {
            if (producer == null)
                throw new InvalidProductProducerException("Product producer can not be null.");
            
            if (Producer.Name.EqualsCaseInvariant(producer.Name))
                return false;

            Producer = producer;
            SetUpdatedDate();
            AddDomainEvent(new ProductProducerChangedDomainEvent(Id, producer));
            return true;
        }

        internal bool AddOrChangePicture(Maybe<Picture> picture)
        {
            var validatePicture = ValidatePicture(picture);

            if (!Picture.IsEmpty && Picture.Equals(validatePicture))
            {
                return false;
            }

            Picture = validatePicture;
            SetUpdatedDate();
            AddDomainEvent(new PictureAddedOrChangedDomainEvent(Id, validatePicture));
            return true;
        }

        internal void RemovePicture()
        {
            if (Picture.IsEmpty)
            {
                throw new CanNotRemoveEmptyPictureException(
                    "Unable to remove picture, because it's already empty.");
            }

            Picture = Picture.Empty;
            SetUpdatedDate();
            AddDomainEvent(new PictureRemovedDomainEvent(Id));
        }

        internal void AssignCategory(CategoryId categoryId)
        {
            var assignedCategory = GetProductCategory(categoryId);

            if (assignedCategory.HasValue)
            {
                throw new ProductIsAlreadyAssignedToCategoryException(
                    $"Product already assigned to category with id: {categoryId.Id}.");
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
                throw new ProductWithAssignedCategoryNotFoundException(
                    $"Product with assigned category with id: {categoryId.Id} not found.");
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
                throw new AnyProductWithAssignedCategoryNotFoundException(
                    "Unable to find any product with assigned category.");
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

        internal static Product Create(ProductId id, CreatorId creatorId, string name, ProductProducer producer)
        {
            return new Product(id, creatorId, name, producer);
        }

        private static string ValidateName(string name)
        {
            if (IsProductNameRequired && name.IsEmpty())
            {
                throw new InvalidProductNameException("Product name can not be empty.");
            }

            if (name.IsLongerThan(ProductNameMaxLength))
            {
                throw new InvalidProductNameException(
                    $"Product name can not be longer than {ProductNameMaxLength} characters.");
            }

            if (name.IsShorterThan(ProductNameMinLength))
            {
                throw new InvalidProductNameException(
                    $"Product name can not be shorter than {ProductNameMinLength} characters.");
            }

            return name;
        }

        private static Picture ValidatePicture(Maybe<Picture> picture)
        {
            if (picture.HasNoValue || picture.Value.IsEmpty)
            {
                throw new PictureCanNotBeEmptyException("Picture can not be empty.");
            }

            return picture.Value;
        }

        private Maybe<ProductCategory> GetProductCategory(CategoryId categoryId)
        {
            var assignedCategory = _productCategories.FirstOrDefault(pr => pr.SecondKey.Equals(categoryId));

            return assignedCategory;
        }
    }
}