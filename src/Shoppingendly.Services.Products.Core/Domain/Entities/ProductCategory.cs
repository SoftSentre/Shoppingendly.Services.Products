﻿using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.Entities
{
    public class ProductCategory : AuditableAndEventSourcingDoubleKeyEntity<ProductId, CategoryId>
    {
        // Navigation property
        public Product Product { get; private set; }
        
        // Navigation property
        public Category Category { get; private set; }

        private ProductCategory()
        {
            // Required for EF
        }

        public ProductCategory(ProductId productId, CategoryId categoryId) 
            : base(productId, categoryId)
        {
        }

        public static ProductCategory Create(ProductId productId, CategoryId categoryId)
        {
            return new ProductCategory(productId, categoryId);
        }
    }
}