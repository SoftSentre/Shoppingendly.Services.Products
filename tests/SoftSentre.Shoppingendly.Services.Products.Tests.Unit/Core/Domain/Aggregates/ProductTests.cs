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

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Aggregates
{
    public class ProductTests
    {
        [Theory]
        [InlineData("Prod")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetProductNameMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
            string name)
        {
            // Arrange
            var productName = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Func<bool> func = () => product.SetName(productName);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(string name)
        {
            // Arrange
            var productName = name;
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be empty.");
        }

        [Theory]
        [InlineData("Prod")]
        [InlineData("IProvideMaximalNumberOfLetters")]
        public void CheckIfSetProductProducerMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
            string name)
        {
            // Arrange
            var productProducer = ProductProducer.CreateProductProducer(name);
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Func<bool> func = () => product.SetProducer(productProducer);
            var testResult = func.Invoke();

            // Assert
            func.Should().NotThrow();
            testResult.Should().BeTrue();
        }
        
        [Fact]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Func<bool> func = () => product.SetProducer(null);

            // Assert
            func.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be null.");
        }

        [Fact]
        public void CheckIfAddOrChangePictureMethodAssignValidObjectWhenInputIsCorrectAndDoNotThrown()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            var picture = Picture.Create("ExamplePictureName", "ExamplePictureUrl");

            // Act
            Func<bool> function = () => product.AddOrChangePicture(picture);
            var testResult = function.Invoke();

            // Assert
            function.Should().NotThrow();
            product.Picture.Should().Be(picture);
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfAddOrChangePictureMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.AddOrChangePicture(Picture.Create("ExamplePictureName", "ExamplePictureUrl"));
            var productNameChangedDomainEvent =
                product.GetUncommitted().LastOrDefault() as PictureAddedOrChangedDomainEvent ??
                It.IsAny<PictureAddedOrChangedDomainEvent>();

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productNameChangedDomainEvent.Should().BeOfType<PictureAddedOrChangedDomainEvent>();
            productNameChangedDomainEvent.Should().NotBeNull();
            productNameChangedDomainEvent.ProductId.Should().Be(product.Id);
            productNameChangedDomainEvent.Picture.Should().Be(product.Picture);
        }

        [Fact]
        public void CheckIfAddOrChangePictureThrowAppropriateExceptionWhenPictureIsEmpty()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            var picture = Picture.Empty;

            // Act
            Func<bool> function = () => product.AddOrChangePicture(picture);

            // Assert
            function.Should().Throw<PictureCanNotBeEmptyException>().WithMessage("Picture can not be empty.");
        }

        [Fact]
        public void CheckIfAssignCategoryMethodDoNotThrowExceptionAndAddCorrectItemToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            var expectedValue = new ProductCategory(product.Id, categoryId);

            // Act
            Action action = () => product.AssignCategory(categoryId);

            // Assert
            action.Should().NotThrow();
            product.ProductCategories.Should().NotBeEmpty();
            product.UpdatedDate.Should().NotBe(default);
            var assignedProduct = product.ProductCategories.FirstOrDefault() ?? It.IsAny<ProductCategory>();
            assignedProduct.FirstKey.Should().Be(expectedValue.FirstKey);
            assignedProduct.SecondKey.Should().Be(expectedValue.SecondKey);
        }

        [Fact]
        public void CheckIfAssignCategoryMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.AssignCategory(categoryId);
            var productAssignedToCategory =
                product.GetUncommitted().LastOrDefault() as ProductAssignedToCategoryDomainEvent ??
                It.IsAny<ProductAssignedToCategoryDomainEvent>();

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productAssignedToCategory.Should().BeOfType<ProductAssignedToCategoryDomainEvent>();
            productAssignedToCategory.Should().NotBeNull();
            productAssignedToCategory.ProductId.Should().Be(product.Id);
            productAssignedToCategory.CategoryId.Should().Be(categoryId);
        }

        [Fact]
        public void CheckIfAssignCategoryMethodThrowExceptionWhenProductIsAlreadyAssignedToCategory()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            var newProductCategory = new ProductCategory(product.Id, categoryId);
            product.ProductCategories.Add(newProductCategory);

            // Act
            Action action = () => product.AssignCategory(categoryId);

            // Assert
            action.Should().Throw<ProductIsAlreadyAssignedToCategoryException>()
                .WithMessage($"Product already assigned to category with id: {categoryId.Id}.");
        }

        [Fact]
        public void CheckIfClearDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.ClearDomainEvents();

            // Assert
            product.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void CheckIfCreateNewProductProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange

            // Act
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            var newCategoryCreatedDomainEvent =
                product.GetUncommitted().LastOrDefault() as NewProductCreatedDomainEvent ??
                It.IsAny<NewProductCreatedDomainEvent>();

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
        public void CheckIfDeallocateAllCategoriesMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());

            // Act
            product.DeallocateAllCategories();
            var productDeallocatedFromAllCategories =
                product.GetUncommitted().LastOrDefault() as ProductDeallocatedFromAllCategoriesDomainEvent ??
                It.IsAny<ProductDeallocatedFromAllCategoriesDomainEvent>();

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productDeallocatedFromAllCategories.Should().BeOfType<ProductDeallocatedFromAllCategoriesDomainEvent>();
            productDeallocatedFromAllCategories.Should().NotBeNull();
            productDeallocatedFromAllCategories.ProductId.Should().Be(product.Id);
            productDeallocatedFromAllCategories.CategoriesIds.Should().HaveCount(2);
        }

        [Fact]
        public void CheckIfDeallocateCategoryMethodDoNotThrowExceptionAndRemoveCorrectItemToList()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            product.AssignCategory(categoryId);

            // Act
            Action action = () => product.DeallocateCategory(categoryId);

            // Assert
            action.Should().NotThrow();
            product.ProductCategories.Should().BeEmpty();
            product.UpdatedDate.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfDeallocateCategoryMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            product.AssignCategory(categoryId);

            // Act
            product.DeallocateCategory(categoryId);
            var productDeallocatedFromCategory =
                product.GetUncommitted().LastOrDefault() as ProductDeallocatedFromCategoryDomainEvent ??
                It.IsAny<ProductDeallocatedFromCategoryDomainEvent>();

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productDeallocatedFromCategory.Should().BeOfType<ProductDeallocatedFromCategoryDomainEvent>();
            productDeallocatedFromCategory.Should().NotBeNull();
            productDeallocatedFromCategory.ProductId.Should().Be(product.Id);
            productDeallocatedFromCategory.CategoryId.Should().Be(categoryId);
        }

        [Fact]
        public void CheckIfDeallocateCategoryMethodThrowExceptionWhenCategoryIsNotFound()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Action action = () => product.DeallocateCategory(categoryId);

            // Assert
            action.Should().Throw<ProductWithAssignedCategoryNotFoundException>()
                .WithMessage($"Product with assigned category with id: {categoryId.Id} not found.");
        }

        [Fact]
        public void CheckIfDeallocateFromAllCategoriesMethodDoNotThrowExceptionAndRemoveAllItemsFromList()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());
            var assignedCategoriesCount = product.ProductCategories.Count;

            // Act
            Action action = () => product.DeallocateAllCategories();

            // Assert
            action.Should().NotThrow();
            assignedCategoriesCount.Should().Be(2);
            product.ProductCategories.Should().BeEmpty();
            product.UpdatedDate.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfDeallocateFromAllCategoriesMethodThrowExceptionWhenCategoryIsNotFound()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Action action = () => product.DeallocateAllCategories();

            // Assert
            action.Should().Throw<AnyProductWithAssignedCategoryNotFoundException>()
                .WithMessage("Unable to find any product with assigned category.");
        }

        [Fact]
        public void CheckIfGetAllAssignedCategoriesMethodReturnValuesAndDoNotThrown()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            product.AssignCategory(new CategoryId());
            product.AssignCategory(new CategoryId());

            // Act
            Func<Maybe<IEnumerable<ProductCategory>>> func = () => product.GetAllAssignedCategories();

            // Assert
            func.Should().NotThrow();
            var assignedCategories = func.Invoke();
            assignedCategories.Value.Should().HaveCount(2);
        }

        [Fact]
        public void CheckIfGetAssignCategoryMethodReturnValidObjectWhenCorrectValueWasProvided()
        {
            // Arrange
            var categoryId = new CategoryId();
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));
            var expectedValue = new ProductCategory(product.Id, categoryId);
            product.ProductCategories.Add(expectedValue);

            // Act
            Func<Maybe<ProductCategory>> func = () => product.GetAssignedCategory(categoryId);
            var assignedCategory = func.Invoke();

            // Assert
            func.Should().NotThrow();
            assignedCategory.Should().NotBeNull();
            assignedCategory.Value.FirstKey.Should().Be(product.Id);
            assignedCategory.Value.SecondKey.Should().Be(categoryId);
        }

        [Fact]
        public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            var domainEvents = product.GetUncommitted().ToList();

            // Assert
            domainEvents.Should().NotBeNull();
            domainEvents.LastOrDefault().Should().BeOfType<NewProductCreatedDomainEvent>();
        }

        [Fact]
        public void CheckIfIsPossibleToRemovePictureIfItIsEmpty()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Action action = () => product.RemovePicture();

            // Assert
            action.Should().Throw<CanNotRemoveEmptyPictureException>()
                .WithMessage("Unable to remove picture, because it's already empty.");
        }

        [Fact]
        public void CheckIfRemovePictureMethodCanDeleteThePictureFromProductAndDoNotThrown()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(),
                Picture.Create("ExamplePictureName", "ExamplePictureUrl"), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Action action = () => product.RemovePicture();

            // Assert
            action.Should().NotThrow();
            product.Picture.Name.Should().Be(null);
            product.Picture.Url.Should().Be(null);
            product.Picture.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void CheckIfRemovePictureMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(),
                Picture.Create("ExamplePictureName", "ExamplePictureUrl"), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.RemovePicture();
            var productNameChangedDomainEvent =
                product.GetUncommitted().LastOrDefault() as PictureRemovedDomainEvent ??
                It.IsAny<PictureRemovedDomainEvent>();

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productNameChangedDomainEvent.Should().BeOfType<PictureRemovedDomainEvent>();
            productNameChangedDomainEvent.Should().NotBeNull();
            productNameChangedDomainEvent.ProductId.Should().Be(product.Id);
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.SetProducer(ProductProducer.CreateProductProducer("Other producer"));
            var productProducerChanged =
                product.GetUncommitted().LastOrDefault() as ProductProducerChangedDomainEvent ??
                It.IsAny<ProductProducerChangedDomainEvent>();

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productProducerChanged.Should().BeOfType<ProductProducerChangedDomainEvent>();
            productProducerChanged.Should().NotBeNull();
            productProducerChanged.ProductId.Should().Be(product.Id);
            productProducerChanged.ProductProducer.Should().Be(product.Producer);
        }

        [Fact]
        public void CheckIfSetNameMethodReturnFalseWhenInputIsTheSameThenExistingValue()
        {
            // Arrange
            const string productName = "ExampleProductName";
            var product = new Product(new ProductId(), new CreatorId(), productName,
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            var testResult = product.SetName(productName);

            // Assert
            testResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfSetNameMethodReturnTrueWhenInputIsDifferentThenExistingValue()
        {
            // Arrange
            const string productName = "ExampleProductName";
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            var testResult = product.SetName(productName);

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductNameMethodProduceDomainEventWithAppropriateTypeAndValues()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.SetName("NewProductName");
            var productNameChangedDomainEvent =
                product.GetUncommitted().LastOrDefault() as ProductNameChangedDomainEvent ??
                It.IsAny<ProductNameChangedDomainEvent>();

            // Assert
            product.DomainEvents.Should().NotBeEmpty();
            productNameChangedDomainEvent.Should().BeOfType<ProductNameChangedDomainEvent>();
            productNameChangedDomainEvent.Should().NotBeNull();
            productNameChangedDomainEvent.ProductId.Should().Be(product.Id);
            productNameChangedDomainEvent.ProductName.Should().Be(product.Name);
        }

        [Fact]
        public void CheckIfSetProductNameMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            const string productName = "OtherProductName";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.SetName(productName);

            // Assert
            product.Name.Should().Be(productName);
            product.UpdatedDate.Should().NotBe(default);
            product.CreatedAt.Should().NotBe(default);
        }

        [Fact]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange
            const string productName = "IProvideMaximalNumberOfLettersAndFewMore";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be longer than 30 characters.");
        }

        [Fact]
        public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            const string productName = "Hom";
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            Func<bool> func = () => product.SetName(productName);

            // Assert
            func.Should().Throw<InvalidProductNameException>()
                .WithMessage("Product name can not be shorter than 4 characters.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodReturnFalseWhenInputIsTheSameThenExistingValue()
        {
            // Arrange
            var productProducer = ProductProducer.CreateProductProducer("ExampleProductProducer");
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", productProducer);

            // Act
            var testResult = product.SetProducer(productProducer);

            // Assert
            testResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfSetProductProducerMethodReturnTrueWhenInputIsDifferentThenExistingValue()
        {
            // Arrange
            var productProducer = ProductProducer.CreateProductProducer("ExampleProductProducer");
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductProducer",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            var testResult = product.SetProducer(productProducer);

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfSetProductProducerMethodSetValuesWhenCorrectNameHasBeenProvided()
        {
            // Arrange
            var productProducer = ProductProducer.CreateProductProducer("OtherProducerName");
            var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
                ProductProducer.CreateProductProducer("ExampleProducer"));

            // Act
            product.SetProducer(productProducer);

            // Assert
            product.Producer.Should().Be(productProducer);
            product.UpdatedDate.Should().NotBe(default);
            product.CreatedAt.Should().NotBe(default);
        }
    }
}