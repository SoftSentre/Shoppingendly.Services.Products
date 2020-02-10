using System;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Events.Categories;
using Shoppingendly.Services.Products.Core.Domain.Events.Products;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Products;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Aggregates
{
    public class ProductTests
    {
        #region domain logic

        [Fact]
        public void CheckIfSetNameMethodReturnTrueWhenInputIsDifferentThenExistingValue()
        {
            // Arrange
            const string productName = "ExampleProductName";
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName", "ExampleProducer");

            // Act
            var testResult = product.SetName(productName);

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetNameMethodReturnFalseWhenInputIsTheSameThenExistingValue()
        {
            // Arrange
            const string productName = "ExampleProductName";
            var product = new Product(new ProductId(), new CreatorId(), productName, "ExampleProducer");

            // Act
            var testResult = product.SetName(productName);

            // Assert
            testResult.Should().BeFalse();
        }

        [Theory]
        [InlineData("Prod")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetProductNameMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
            string name)
        {
            // Arrange
            var productName = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductNameMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string productName = "OtherProductName";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetName(productName);

            // Assert
            product.Name.Should().Be(productName);
            product.UpdatedDate.Should().NotBe(default);
            product.CreatedAt.Should().NotBe(default);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(string name)
        {
            // Arrange
            var productName = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be empty.");
        }

        [Fact]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            const string productName = "Hom";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be shorter than 4 characters.");
        }

        [Fact]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange
            const string productName = "IProvideMaximalNumberOfLettersAndFewMore";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be longer than 30 characters.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodReturnTrueWhenInputIsDifferentThenExistingValue()
        {
            // Arrange
            const string productProducer = "ExampleProductProducer";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductProducer", "ExampleProducer");

            // Act
            var testResult = product.SetProducer(productProducer);

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductProducerMethodReturnFalseWhenInputIsTheSameThenExistingValue()
        {
            // Arrange
            const string productProducer = "ExampleProductProducer";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", productProducer);

            // Act
            var testResult = product.SetProducer(productProducer);

            // Assert
            testResult.Should().BeFalse();
        }

        [Theory]
        [InlineData("Prod")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetProductProducerMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
            string name)
        {
            // Arrange
            var productProducer = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(productProducer);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductProducerMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string productProducer = "OtherProducerName";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetProducer(productProducer);

            // Assert
            product.Producer.Should().Be(productProducer);
            product.UpdatedDate.Should().NotBe(default);
            product.CreatedAt.Should().NotBe(default);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(
            string name)
        {
            // Arrange
            var productProducer = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(productProducer);

            // Assert
            func.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be empty.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            const string producerName = "P";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(producerName);

            // Assert
            func.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be shorter than 2 characters.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange
            const string producerName =
                "IProvideMaximalNumberOfLettersAndFewMoreBecauseProducerCanNotBeLongerThan50Characters";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            Func<bool> func = () => product.SetProducer(producerName);

            // Assert
            func.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be longer than 50 characters.");
        }

        #endregion

        #region domain events

        [Fact]
        public void CheckIfCreateNewProductProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");
            var newCategoryCreatedDomainEvent =
                product.GetUncommitted().LastOrDefault() as NewProductCreatedDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            newCategoryCreatedDomainEvent.Should().BeOfType<NewProductCreatedDomainEvent>();
            newCategoryCreatedDomainEvent.Should().NotBeNull();
            newCategoryCreatedDomainEvent.ProductId.Should().Be(product.Id);
            newCategoryCreatedDomainEvent.CreatorId.Should().Be(product.CreatorId);
            newCategoryCreatedDomainEvent.ProductName.Should().Be(product.Name);
            newCategoryCreatedDomainEvent.ProductProducer.Should().Be(product.Producer);
        }

        [Fact]
        public void CheckIfSetProductNameMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetName("NewProductName");
            var productNameChangedDomainEvent =
                product.GetUncommitted().LastOrDefault() as ProductNameChangedDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productNameChangedDomainEvent.Should().BeOfType<ProductNameChangedDomainEvent>();
            productNameChangedDomainEvent.Should().NotBeNull();
            productNameChangedDomainEvent.ProductId.Should().Be(product.Id);
            productNameChangedDomainEvent.ProductName.Should().Be(product.Name);
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.SetProducer("Other producer");
            var productProducerChanged =
                product.GetUncommitted().LastOrDefault() as ProductProducerChangedDomainEvent;

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productProducerChanged.Should().BeOfType<ProductProducerChangedDomainEvent>();
            productProducerChanged.Should().NotBeNull();
            productProducerChanged.ProductId.Should().Be(product.Id);
            productProducerChanged.ProductProducer.Should().Be(product.Producer);
        }

        [Fact]
        public void CheckIfClearDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            product.ClearDomainEvents();

            // Assert
            product.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", "ExampleProducer");

            // Act
            var domainEvents = product.GetUncommitted().ToList();

            // Assert
            domainEvents.Should().NotBeNull();
            domainEvents.LastOrDefault().Should().BeOfType<NewProductCreatedDomainEvent>();
        }

        #endregion
    }
}