using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Products;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Products;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Core.Domain.Aggregates
{
    public class Product : AuditableAndEventSourcingEntity<ProductId>, IAggregateRoot
    {
        private HashSet<ProductCategory> _productCategories;

        public CreatorId CreatorId { get; }
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
            Name = ValidateName(name);
            Producer = ValidateProducer(producer);
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, name, producer));
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
    }
}