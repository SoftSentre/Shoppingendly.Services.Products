using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.ProductCategories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Entities
{
    public class ProductCategoryTests
    {
        [Fact]
        public void CheckIfCreateNewProductCategoryByConstructorProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var productCategory = new ProductCategory(new ProductId(), new CategoryId());
            var newProductCategoryCreatedDomainEvent = productCategory.GetUncommitted().LastOrDefault() as ProductAssignedToCategoryDomainEvent;
            
            // Assert
            productCategory.DomainEvents.Should().NotBeEmpty();
            newProductCategoryCreatedDomainEvent.Should().BeOfType<ProductAssignedToCategoryDomainEvent>();
            newProductCategoryCreatedDomainEvent.Should().NotBeNull();
            newProductCategoryCreatedDomainEvent.ProductId.Should().Be(productCategory.FirstKey);
            newProductCategoryCreatedDomainEvent.CategoryId.Should().Be(productCategory.SecondKey);
        }
        
        [Fact]
        public void CheckIfClearDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var productCategory = new ProductCategory(new ProductId(), new CategoryId());

            // Act
            productCategory.ClearDomainEvents();

            // Assert
            productCategory.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var productCategory = new ProductCategory(new ProductId(), new CategoryId());
            
            // Act
            var domainEvents = productCategory.GetUncommitted().ToList();
            
            // Assert
            domainEvents.Should().NotBeNull();
            domainEvents.LastOrDefault().Should().BeOfType<ProductAssignedToCategoryDomainEvent>();
        }
    }
}