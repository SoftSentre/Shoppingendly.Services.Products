using System;
using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Products;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

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
            Name = name;
            Producer = producer;
            AddDomainEvent(new NewProductCreatedDomainEvent(id, creatorId, name, producer));
        }

        public bool SetName(string name)
        {
            AddDomainEvent(new ProductNameChangedDomainEvent(Id, name));
            return true;
        }

        public bool SetProducer(string producer)
        {
            AddDomainEvent(new ProductProducerChangedDomainEvent(Id, producer));
            return true;
        }

        public static Product Create(ProductId id, CreatorId creatorId, string name, string producer)
        {
            return new Product(id, creatorId, name, producer);
        }

        private string ValidateName(string name)
        {
            throw new NotImplementedException();
        }

        private string ValidateProducer(string producer)
        {
            throw  new NotImplementedException();
        }
    }
}