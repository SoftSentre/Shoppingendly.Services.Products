using System;
using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Products.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Core.Domain.Products.Entities
{
    public class Category : AuditableAndEventSourcingEntity<CategoryId>
    {
        private HashSet<ProductCategory> _productCategories;

        public string Name { get; private set; }
        public string Description { get; private set; }

        public HashSet<ProductCategory> ProductCategories
        {
            get => _productCategories;
            set => _productCategories = new HashSet<ProductCategory>(value);
        }

        private Category()
        {
            // Required for EF
        }

        public Category(CategoryId categoryId, string name, string description) : base(categoryId)
        {
            Name = name;
            Description = description;
        }

        public bool SetName(string name)
        {
            if (name.IsEmpty())
                throw new InvalidCategoryNameException("Category name can not be empty.", name);
            if (name.IsLongerThan(30))
                throw new InvalidCategoryNameException("Category name can not be longer than 30 characters.", name);
            if (name.IsShorterThan(4))
                throw new InvalidCategoryNameException("Category name can not be shorter than 4 characters.", name);
            
            return !Name.EqualsCaseInvariant(name);
        }

        public bool SetDescription(string description)
        {
            throw new NotImplementedException();
        }

        public static Category Create(CategoryId categoryId, string name, string description)
        {
            return new Category(categoryId, name, description);
        }
    }
}