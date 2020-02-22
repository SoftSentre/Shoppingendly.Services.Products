using System.Collections.Generic;
using Shoppingendly.Services.Products.Core.Domain.Base.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Categories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Categories;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Core.Domain.Entities
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

        public Category(CategoryId categoryId, string name) : base(categoryId)
        {
            Name = ValidateCategoryName(name);
            AddDomainEvent(new NewCategoryCreatedDomainEvent(Id, name));
        }
        
        public Category(CategoryId categoryId, string name, string description) : base(categoryId)
        {
            Name = ValidateCategoryName(name);
            Description = ValidateCategoryDescription(description);
            AddDomainEvent(new NewCategoryCreatedDomainEvent(categoryId, name, description));
        }

        public bool SetName(string name)
        {
            ValidateCategoryName(name);
            
            if (Name.EqualsCaseInvariant(name)) 
                return false;
            
            Name = name;
            SetUpdatedDate();
            AddDomainEvent(new CategoryNameChangedDomainEvent(Id, name));
            return true;
        }

        public bool SetDescription(string description)
        {
            ValidateCategoryDescription(description);
            
            if (Description.EqualsCaseInvariant(description)) 
                return false;
            
            Description = description;
            SetUpdatedDate();
            AddDomainEvent(new CategoryDescriptionChangedDomainEvent(Id, description));
            return true;
        }
        
        public static Category Create(CategoryId categoryId, string name)
        {
            return new Category(categoryId, name);
        }
        
        public static Category Create(CategoryId categoryId, string name, string description)
        {
            return new Category(categoryId, name, description);
        }

        private static string ValidateCategoryName(string name)
        {
            if (name.IsEmpty())
                throw new InvalidCategoryNameException("Category name can not be empty.");
            if (name.IsLongerThan(30))
                throw new InvalidCategoryNameException("Category name can not be longer than 30 characters.");
            if (name.IsShorterThan(4))
                throw new InvalidCategoryNameException("Category name can not be shorter than 4 characters.");

            return name;
        }

        private static string ValidateCategoryDescription(string description)
        {
            if (description.IsShorterThan(20))
                throw new InvalidCategoryDescriptionException(
                    "Category description can not be shorter than 20 characters.");
            if (description.IsLongerThan(4000))
                throw new InvalidCategoryDescriptionException(
                    "Category description can not be longer than 4000 characters.");

            return description;
        }
    }
}