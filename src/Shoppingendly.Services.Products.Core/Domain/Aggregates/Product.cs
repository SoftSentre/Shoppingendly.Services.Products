using System.Collections.Generic;
using System.Linq;
using Shoppingendly.Services.Products.Core.Domain.Base.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Products;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Products;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Aggregates
{
    public class Product : AuditableAndEventSourcingEntity<ProductId>, IAggregateRoot
    {
        private HashSet<ProductCategory> _productCategories = new HashSet<ProductCategory>();

        public CreatorId CreatorId { get; }
        public Picture Picture { get; private set; }
        public string Name { get; private set; }
        public string Producer { get; private set; }

        // Navigation property
        public Creator Creator { get; set; }

        public HashSet<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set => _productCategories = new HashSet<ProductCategory>(value);
        }

        private Product()
        {
            // Required for EF
        }

        public Product(ProductId id, CreatorId creatorId, string name, string producer) : base(id)
        {
            CreatorId = creatorId;
            Picture = Picture.Empty;
            Name = ValidateName(name);
            Producer = ValidateProducer(producer);
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, name, producer, Picture.Empty));
        }

        public Product(ProductId id, CreatorId creatorId, Picture picture, string name, string producer) : base(id)
        {
            CreatorId = creatorId;
            Picture = picture;
            Name = ValidateName(name);
            Producer = ValidateProducer(producer);
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, name, producer, picture));
        }

        public bool SetName(string name)
        {
            ValidateName(name);

            if (Name.EqualsCaseInvariant(name))
                return false;

            Name = name;
            SetUpdatedDate();
            AddDomainEvent(new ProductNameChangedDomainEvent(Id, name));
            return true;
        }

        public bool SetProducer(string producer)
        {
            ValidateProducer(producer);

            if (Producer.EqualsCaseInvariant(producer))
                return false;

            Producer = producer;
            SetUpdatedDate();
            AddDomainEvent(new ProductProducerChangedDomainEvent(Id, producer));
            return true;
        }

        public bool AddOrChangePicture(Maybe<Picture> picture)
        {
            var validatePicture = ValidatePicture(picture);

            if (Picture.Equals(validatePicture))
                return false;
            
            Picture = validatePicture;
            SetUpdatedDate();
            AddDomainEvent(new PictureAddedOrChangedDomainEvent(Id, validatePicture));
            return true;
        }

        public void RemovePicture()
        {
            if (Picture.IsEmpty)
                throw new CanNotRemoveEmptyPictureException("Unable to remove picture, because it's already empty.");
            
            Picture = Picture.Empty;
            SetUpdatedDate();
            AddDomainEvent(new PictureRemovedDomainEvent(Id));
        }

        public void AssignCategory(CategoryId categoryId)
        {
            var assignedCategory = GetProductCategory(categoryId);

            if (assignedCategory.HasValue)
                throw new ProductIsAlreadyAssignedToCategoryException(
                    $"Product already assigned to category with id: {categoryId.Id}.");

            var newAssignedCategory = new ProductCategory(Id, categoryId);
            _productCategories.Add(newAssignedCategory);
            SetUpdatedDate();
            AddDomainEvent(new ProductAssignedToCategoryDomainEvent(newAssignedCategory.FirstKey,
                newAssignedCategory.SecondKey));
        }

        public void DeallocateCategory(CategoryId categoryId)
        {
            var assignedCategory = GetProductCategory(categoryId);

            if (assignedCategory.HasNoValue)
                throw new ProductWithAssignedCategoryNotFoundException(
                    $"Product with assigned category with id: {categoryId.Id} not found.");

            _productCategories.Remove(assignedCategory.Value);
            SetUpdatedDate();
            AddDomainEvent(new ProductDeallocatedFromCategoryDomainEvent(assignedCategory.Value.FirstKey,
                assignedCategory.Value.SecondKey));
        }

        public void DeallocateAllCategories()
        {
            if (!_productCategories.Any())
                throw new AnyProductWithAssignedCategoryNotFoundException(
                    "Unable to find any product with assigned category.");

            var categoriesIds = _productCategories.Select(pc => pc.SecondKey).ToList();
            _productCategories.Clear();
            SetUpdatedDate();
            AddDomainEvent(new ProductDeallocatedFromAllCategoriesDomainEvent(Id, categoriesIds));
        }

        public Maybe<IEnumerable<ProductCategory>> GetAllAssignedCategories()
        {
            return _productCategories;
        }

        public Maybe<ProductCategory> GetAssignedCategory(CategoryId categoryId)
        {
            return GetProductCategory(categoryId);
        }

        public static Product Create(ProductId id, CreatorId creatorId, string name, string producer)
        {
            return new Product(id, creatorId, name, producer);
        }

        private static string ValidateName(string name)
        {
            if (name.IsEmpty())
                throw new InvalidProductNameException("Product name can not be empty.");
            if (name.IsLongerThan(30))
                throw new InvalidProductNameException("Product name can not be longer than 30 characters.");
            if (name.IsShorterThan(4))
                throw new InvalidProductNameException("Product name can not be shorter than 4 characters.");

            return name;
        }

        private static string ValidateProducer(string producer)
        {
            if (producer.IsEmpty())
                throw new InvalidProductProducerException("Product producer can not be empty.");
            if (producer.IsLongerThan(50))
                throw new InvalidProductProducerException("Product producer can not be longer than 50 characters.");
            if (producer.IsShorterThan(2))
                throw new InvalidProductProducerException("Product producer can not be shorter than 2 characters.");

            return producer;
        }

        private static Picture ValidatePicture(Maybe<Picture> picture)
        {
            if (picture.HasNoValue || picture.Value.IsEmpty)
                throw new PictureCanNotBeEmptyException("Picture can not be empty.");

            return picture.Value;
        }
        
        private Maybe<ProductCategory> GetProductCategory(CategoryId categoryId)
        {
            var assignedCategory = _productCategories.FirstOrDefault(pr => pr.SecondKey.Equals(categoryId));

            return assignedCategory;
        }
    }
}