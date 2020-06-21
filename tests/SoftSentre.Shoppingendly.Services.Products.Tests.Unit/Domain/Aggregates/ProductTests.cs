// // Copyright 2020 SoftSentre Contributors
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// //     http://www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
//
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using FluentAssertions;
// using Moq;
// using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
// using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
// using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
// using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Products;
// using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Producers;
// using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
// using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
// using Xunit;
//
// namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Aggregates
// {
//     public class ProductTests
//     {
//         [Theory]
//         [InlineData("Prod")]
//         [InlineData("IProvideMaximalNumberOfLetters")]
//         public void CheckIfSetProductNameMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
//             string name)
//         {
//             // Arrange
//             var productName = name;
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Func<bool> func = () => product.ChangeProductName(productName);
//             var testResult = func.Invoke();
//
//             // Assert
//             func.Should().NotThrow();
//             testResult.Should().BeTrue();
//         }
//
//         [Theory]
//         [InlineData("")]
//         [InlineData(null)]
//         public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(string name)
//         {
//             // Arrange
//             var productName = name;
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Func<bool> func = () => product.ChangeProductName(productName);
//
//             // Assert
//             func.Should().Throw<ProductNameCanNotBeEmptyException>()
//                 .WithMessage("Product name can not be empty.");
//         }
//
//         [Theory]
//         [InlineData("Prod")]
//         [InlineData("IProvideMaximalNumberOfLetters")]
//         public void CheckIfSetProductProducerMethodReturnTrueWhenCorrectNameHasBeenProvidedAndDoNotThrowAnyException(
//             string name)
//         {
//             // Arrange
//             var productProducer = ProductProducer.CreateProductProducer(name);
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Func<bool> func = () => product.ChangeProductProducer(productProducer);
//             var testResult = func.Invoke();
//
//             // Assert
//             func.Should().NotThrow();
//             testResult.Should().BeTrue();
//         }
//
//         [Fact]
//         public void CheckIfAddOrChangePictureMethodAssignValidObjectWhenInputIsCorrectAndDoNotThrown()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             var picture = Picture.Create("ExamplePictureName", "ExamplePictureUrl");
//
//             // Act
//             Func<bool> function = () => product.UploadProductPicture(picture);
//             var testResult = function.Invoke();
//
//             // Assert
//             function.Should().NotThrow();
//             product.ProductPicture.Should().Be(picture);
//             testResult.Should().BeTrue();
//         }
//
//         [Fact]
//         public void CheckIfAddOrChangePictureMethodProduceDomainEventWithAppropriateTypeAndValues()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             product.UploadProductPicture(Picture.Create("ExamplePictureName", "ExamplePictureUrl"));
//             var productNameChangedDomainEvent =
//                 product.GetUncommitted().LastOrDefault() as ProductPictureUploadedDomainEvent ??
//                 It.IsAny<ProductPictureUploadedDomainEvent>();
//
//             // Assert
//             product.DomainEvents.Should().NotBeEmpty();
//             productNameChangedDomainEvent.Should().BeOfType<ProductPictureUploadedDomainEvent>();
//             productNameChangedDomainEvent.Should().NotBeNull();
//             productNameChangedDomainEvent.ProductId.Should().Be(product.ProductId);
//             productNameChangedDomainEvent.ProductPicture.Should().Be(product.ProductPicture);
//         }
//
//         [Fact]
//         public void CheckIfAddOrChangePictureThrowAppropriateExceptionWhenPictureIsEmpty()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             var picture = Picture.Empty;
//
//             // Act
//             Func<bool> function = () => product.UploadProductPicture(picture);
//
//             // Assert
//             function.Should().Throw<ProductPictureCanNotBeNullOrEmptyException>()
//                 .WithMessage("Product picture can not be null or empty.");
//         }
//
//         [Fact]
//         public void CheckIfAssignCategoryMethodDoNotThrowExceptionAndAddCorrectItemToList()
//         {
//             // Arrange
//             var categoryId = new CategoryId();
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             var expectedValue = new ProductCategory(product.ProductId, categoryId);
//
//             // Act
//             Action action = () => product.AssignCategory(categoryId);
//
//             // Assert
//             action.Should().NotThrow();
//             product.ProductCategories.Should().NotBeEmpty();
//             product.UpdatedDate.Should().NotBe(default);
//             var assignedProduct = product.ProductCategories.FirstOrDefault() ?? It.IsAny<ProductCategory>();
//             assignedProduct.ProductId.Should().Be(expectedValue.ProductId);
//             assignedProduct.CategoryId.Should().Be(expectedValue.CategoryId);
//         }
//
//         [Fact]
//         public void CheckIfAssignCategoryMethodProduceDomainEventWithAppropriateTypeAndValues()
//         {
//             // Arrange
//             var categoryId = new CategoryId();
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             product.AssignCategory(categoryId);
//             var productAssignedToCategory =
//                 product.GetUncommitted().LastOrDefault() as ProductAssignedToCategoryDomainEvent ??
//                 It.IsAny<ProductAssignedToCategoryDomainEvent>();
//
//             // Assert
//             product.DomainEvents.Should().NotBeEmpty();
//             productAssignedToCategory.Should().BeOfType<ProductAssignedToCategoryDomainEvent>();
//             productAssignedToCategory.Should().NotBeNull();
//             productAssignedToCategory.ProductId.Should().Be(product.ProductId);
//             productAssignedToCategory.CategoryId.Should().Be(categoryId);
//         }
//
//         [Fact]
//         public void CheckIfAssignCategoryMethodThrowExceptionWhenProductIsAlreadyAssignedToCategory()
//         {
//             // Arrange
//             var categoryId = new CategoryId();
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             var newProductCategory = new ProductCategory(product.ProductId, categoryId);
//             product.ProductCategories.Add(newProductCategory);
//
//             // Act
//             Action action = () => product.AssignCategory(categoryId);
//
//             // Assert
//             action.Should().Throw<ProductIsAlreadyAssignedToCategoryException>()
//                 .WithMessage($"Product already assigned to category with id: {categoryId}.");
//         }
//
//         [Fact]
//         public void CheckIfClearDomainEventsMethodWorkingProperly()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             product.ClearDomainEvents();
//
//             // Assert
//             product.DomainEvents.Should().BeEmpty();
//         }
//
//         [Fact]
//         public void CheckIfCreateNewProductProduceDomainEventWithAppropriateTypeAndValues()
//         {
//             // Arrange
//
//             // Act
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             var newCategoryCreatedDomainEvent =
//                 product.GetUncommitted().LastOrDefault() as NewProductCreatedDomainEvent ??
//                 It.IsAny<NewProductCreatedDomainEvent>();
//
//             // Assert
//             product.DomainEvents.Should().NotBeEmpty();
//             newCategoryCreatedDomainEvent.Should().BeOfType<NewProductCreatedDomainEvent>();
//             newCategoryCreatedDomainEvent.Should().NotBeNull();
//             newCategoryCreatedDomainEvent.ProductId.Should().Be(product.ProductId);
//             newCategoryCreatedDomainEvent.CreatorId.Should().Be(product.CreatorId);
//             newCategoryCreatedDomainEvent.ProductName.Should().Be(product.ProductName);
//             newCategoryCreatedDomainEvent.ProductProducer.Should().Be(product.ProductProducer);
//         }
//
//         [Fact]
//         public void CheckIfDeallocateAllCategoriesMethodProduceDomainEventWithAppropriateTypeAndValues()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             product.AssignCategory(new CategoryId());
//             product.AssignCategory(new CategoryId());
//
//             // Act
//             product.DeallocateAllCategories();
//             var productDeallocatedFromAllCategories =
//                 product.GetUncommitted().LastOrDefault() as ProductDeallocatedFromAllCategoriesDomainEvent ??
//                 It.IsAny<ProductDeallocatedFromAllCategoriesDomainEvent>();
//
//             // Assert
//             product.DomainEvents.Should().NotBeEmpty();
//             productDeallocatedFromAllCategories.Should().BeOfType<ProductDeallocatedFromAllCategoriesDomainEvent>();
//             productDeallocatedFromAllCategories.Should().NotBeNull();
//             productDeallocatedFromAllCategories.ProductId.Should().Be(product.ProductId);
//             productDeallocatedFromAllCategories.CategoriesIds.Should().HaveCount(2);
//         }
//
//         [Fact]
//         public void CheckIfDeallocateCategoryMethodDoNotThrowExceptionAndRemoveCorrectItemToList()
//         {
//             // Arrange
//             var categoryId = new CategoryId();
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             product.AssignCategory(categoryId);
//
//             // Act
//             Action action = () => product.DeallocateCategory(categoryId);
//
//             // Assert
//             action.Should().NotThrow();
//             product.ProductCategories.Should().BeEmpty();
//             product.UpdatedDate.Should().NotBe(default);
//         }
//
//         [Fact]
//         public void CheckIfDeallocateCategoryMethodProduceDomainEventWithAppropriateTypeAndValues()
//         {
//             // Arrange
//             var categoryId = new CategoryId();
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             product.AssignCategory(categoryId);
//
//             // Act
//             product.DeallocateCategory(categoryId);
//             var productDeallocatedFromCategory =
//                 product.GetUncommitted().LastOrDefault() as ProductDeallocatedFromCategoryDomainEvent ??
//                 It.IsAny<ProductDeallocatedFromCategoryDomainEvent>();
//
//             // Assert
//             product.DomainEvents.Should().NotBeEmpty();
//             productDeallocatedFromCategory.Should().BeOfType<ProductDeallocatedFromCategoryDomainEvent>();
//             productDeallocatedFromCategory.Should().NotBeNull();
//             productDeallocatedFromCategory.ProductId.Should().Be(product.ProductId);
//             productDeallocatedFromCategory.CategoryId.Should().Be(categoryId);
//         }
//
//         [Fact]
//         public void CheckIfDeallocateCategoryMethodThrowExceptionWhenCategoryIsNotFound()
//         {
//             // Arrange
//             var categoryId = new CategoryId();
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Action action = () => product.DeallocateCategory(categoryId);
//
//             // Assert
//             action.Should().Throw<ProductWithAssignedCategoryNotFoundException>()
//                 .WithMessage($"Product with assigned category with id: {categoryId} not found.");
//         }
//
//         [Fact]
//         public void CheckIfDeallocateFromAllCategoriesMethodDoNotThrowExceptionAndRemoveAllItemsFromList()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             product.AssignCategory(new CategoryId());
//             product.AssignCategory(new CategoryId());
//             var assignedCategoriesCount = product.ProductCategories.Count;
//
//             // Act
//             Action action = () => product.DeallocateAllCategories();
//
//             // Assert
//             action.Should().NotThrow();
//             assignedCategoriesCount.Should().Be(2);
//             product.ProductCategories.Should().BeEmpty();
//             product.UpdatedDate.Should().NotBe(default);
//         }
//
//         [Fact]
//         public void CheckIfDeallocateFromAllCategoriesMethodThrowExceptionWhenCategoryIsNotFound()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Action action = () => product.DeallocateAllCategories();
//
//             // Assert
//             action.Should().Throw<ProductWithAssignedCategoriesNotFoundException>()
//                 .WithMessage("Unable to find any product with assigned categories.");
//         }
//
//         [Fact]
//         public void CheckIfGetAllAssignedCategoriesMethodReturnValuesAndDoNotThrown()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             product.AssignCategory(new CategoryId());
//             product.AssignCategory(new CategoryId());
//
//             // Act
//             Func<Maybe<IEnumerable<ProductCategory>>> func = () => product.GetAllAssignedCategories();
//
//             // Assert
//             func.Should().NotThrow();
//             var assignedCategories = func.Invoke();
//             assignedCategories.Value.Should().HaveCount(2);
//         }
//
//         [Fact]
//         public void CheckIfGetAssignCategoryMethodReturnValidObjectWhenCorrectValueWasProvided()
//         {
//             // Arrange
//             var categoryId = new CategoryId();
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//             var expectedValue = new ProductCategory(product.ProductId, categoryId);
//             product.ProductCategories.Add(expectedValue);
//
//             // Act
//             Func<Maybe<ProductCategory>> func = () => product.GetAssignedCategory(categoryId);
//             var assignedCategory = func.Invoke();
//
//             // Assert
//             func.Should().NotThrow();
//             assignedCategory.Should().NotBeNull();
//             assignedCategory.Value.ProductId.Should().Be(product.ProductId);
//             assignedCategory.Value.CategoryId.Should().Be(categoryId);
//         }
//
//         [Fact]
//         public void CheckIfGetUncommittedDomainEventsMethodWorkingProperly()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             var domainEvents = product.GetUncommitted().ToList();
//
//             // Assert
//             domainEvents.Should().NotBeNull();
//             domainEvents.LastOrDefault().Should().BeOfType<NewProductCreatedDomainEvent>();
//         }
//         
//         [Fact]
//         public void CheckIfSetCategoryDescriptionMethodProduceDomainEventWithAppropriateTypeAndValues()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             product.ChangeProductProducer(ProductProducer.CreateProductProducer("Other producer"));
//             var productProducerChanged =
//                 product.GetUncommitted().LastOrDefault() as ProductProducerChangedDomainEvent ??
//                 It.IsAny<ProductProducerChangedDomainEvent>();
//
//             // Assert
//             product.DomainEvents.Should().NotBeEmpty();
//             productProducerChanged.Should().BeOfType<ProductProducerChangedDomainEvent>();
//             productProducerChanged.Should().NotBeNull();
//             productProducerChanged.ProductId.Should().Be(product.ProductId);
//             productProducerChanged.ProductProducer.Should().Be(product.ProductProducer);
//         }
//
//         [Fact]
//         public void CheckIfSetNameMethodReturnFalseWhenInputIsTheSameThenExistingValue()
//         {
//             // Arrange
//             const string productName = "ExampleProductName";
//             var product = new Product(new ProductId(), new CreatorId(), productName,
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             var testResult = product.ChangeProductName(productName);
//
//             // Assert
//             testResult.Should().BeFalse();
//         }
//
//         [Fact]
//         public void CheckIfSetNameMethodReturnTrueWhenInputIsDifferentThenExistingValue()
//         {
//             // Arrange
//             const string productName = "ExampleProductName";
//             var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             var testResult = product.ChangeProductName(productName);
//
//             // Assert
//             testResult.Should().BeTrue();
//         }
//
//         [Fact]
//         public void CheckIfSetProductNameMethodProduceDomainEventWithAppropriateTypeAndValues()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             product.ChangeProductName("NewProductName");
//             var productNameChangedDomainEvent =
//                 product.GetUncommitted().LastOrDefault() as ProductNameChangedDomainEvent ??
//                 It.IsAny<ProductNameChangedDomainEvent>();
//
//             // Assert
//             product.DomainEvents.Should().NotBeEmpty();
//             productNameChangedDomainEvent.Should().BeOfType<ProductNameChangedDomainEvent>();
//             productNameChangedDomainEvent.Should().NotBeNull();
//             productNameChangedDomainEvent.ProductId.Should().Be(product.ProductId);
//             productNameChangedDomainEvent.ProductName.Should().Be(product.ProductName);
//         }
//
//         [Fact]
//         public void CheckIfSetProductNameMethodSetValuesWhenCorrectNameHasBeenProvided()
//         {
//             // Arrange
//             const string productName = "OtherProductName";
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             product.ChangeProductName(productName);
//
//             // Assert
//             product.ProductName.Should().Be(productName);
//             product.UpdatedDate.Should().NotBe(default);
//             product.CreatedAt.Should().NotBe(default);
//         }
//
//         [Fact]
//         public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
//         {
//             // Arrange
//             const string productName = "IProvideMaximalNumberOfLettersAndFewMore";
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Func<bool> func = () => product.ChangeProductName(productName);
//
//             // Assert
//             func.Should().Throw<ProductNameIsTooLongException>()
//                 .WithMessage("Product name can not be longer than 30 characters.");
//         }
//
//         [Fact]
//         public void CheckIfSetProductNameMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
//         {
//             // Arrange
//             const string productName = "Hom";
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Func<bool> func = () => product.ChangeProductName(productName);
//
//             // Assert
//             func.Should().Throw<ProductNameIsTooShortException>()
//                 .WithMessage("Product name can not be shorter than 4 characters.");
//         }
//
//         [Fact]
//         public void CheckIfSetProductProducerMethodReturnFalseWhenInputIsTheSameThenExistingValue()
//         {
//             // Arrange
//             var productProducer = ProductProducer.CreateProductProducer("ExampleProductProducer");
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName", productProducer);
//
//             // Act
//             var testResult = product.ChangeProductProducer(productProducer);
//
//             // Assert
//             testResult.Should().BeFalse();
//         }
//
//         [Fact]
//         public void CheckIfSetProductProducerMethodReturnTrueWhenInputIsDifferentThenExistingValue()
//         {
//             // Arrange
//             var productProducer = ProductProducer.CreateProductProducer("ExampleProductProducer");
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductProducer",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             var testResult = product.ChangeProductProducer(productProducer);
//
//             // Assert
//             testResult.Should().BeTrue();
//         }
//
//         [Fact]
//         public void CheckIfSetProductProducerMethodSetValuesWhenCorrectNameHasBeenProvided()
//         {
//             // Arrange
//             var productProducer = ProductProducer.CreateProductProducer("OtherProducerName");
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             product.ChangeProductProducer(productProducer);
//
//             // Assert
//             product.ProductProducer.Should().Be(productProducer);
//             product.UpdatedDate.Should().NotBe(default);
//             product.CreatedAt.Should().NotBe(default);
//         }
//
//         [Fact]
//         public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided()
//         {
//             // Arrange
//             var product = new Product(new ProductId(), new CreatorId(), "ExampleProductName",
//                 ProductProducer.CreateProductProducer("ExampleProducer"));
//
//             // Act
//             Func<bool> func = () => product.ChangeProductProducer(null);
//
//             // Assert
//             func.Should().Throw<ProductProducerCanNotBeNullException>()
//                 .WithMessage("Product producer can not be null.");
//         }
//     }
// }