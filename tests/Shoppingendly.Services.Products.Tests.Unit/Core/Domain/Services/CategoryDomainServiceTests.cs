using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.Services;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services.Categories;
using Shoppingendly.Services.Products.Core.Types;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.Services
{
    public class CategoryDomainServiceTests
    {
        private const string CategoryName = "DefaultCategoryName";
        private const string CategoryDescription = "DefaultCategoryDescription";

        private readonly CategoryId _categoryId;
        private readonly Category _category;

        public CategoryDomainServiceTests()
        {
            _categoryId = new CategoryId();
            _category = Category.Create(_categoryId, "defaultCategoryName");
        }

        [Fact]
        public async Task CheckIfGetCategoryMethodReturnValidObject()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(_category);

            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            var testResult = await categoryDomainService.GetCategoryAsync(_categoryId);

            // Assert
            testResult.Should().Be(_category);
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetCategoryByNameMethodReturnValidObject()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByNameAsync(CategoryName))
                .ReturnsAsync(_category);

            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            var testResult = await categoryDomainService.GetCategoryByNameAsync(CategoryName);

            // Assert
            testResult.Should().Be(_category);
            categoryRepository.Verify(cr => cr.GetByNameAsync(CategoryName), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetCategoryWithIncludesMethodReturnValidObject()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            var category = new Category(_categoryId, CategoryName);
            category.ProductCategories.Add(It.IsAny<ProductCategory>());
            categoryRepository.Setup(cr => cr.GetByNameWithIncludesAsync(CategoryName))
                .ReturnsAsync(category);

            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            var testResult = await categoryDomainService.GetCategoryWithProductsAsync(CategoryName);

            // Assert
            testResult.Should().Be(category);
            categoryRepository.Verify(cr => cr.GetByNameWithIncludesAsync(CategoryName), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetAllCategoriesMethodReturnValidObjects()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryList = new List<Category>
            {
                new Category(new CategoryId(), "ExampleName"),
                new Category(new CategoryId(), "ExampleName2")
            };

            categoryRepository.Setup(cr => cr.GetAllAsync())
                .ReturnsAsync(categoryList);

            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            var testResult = await categoryDomainService.GetAllCategoriesAsync();

            // Assert
            testResult.Should().Be(categoryList);
            categoryRepository.Verify(cr => cr.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CheckIfGetAllCategoriesWithProductsMethodReturnValidObjects()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            var category = new Category(_categoryId, CategoryName);
            category.ProductCategories.Add(It.IsAny<ProductCategory>());

            var categoryList = new List<Category>
            {
                category
            };

            categoryRepository.Setup(cr => cr.GetAllWithIncludesAsync())
                .ReturnsAsync(categoryList);

            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            var testResult = await categoryDomainService.GetAllCategoriesWithProductsAsync();

            // Assert
            testResult.Should().Be(categoryList);
            categoryRepository.Verify(cr => cr.GetAllWithIncludesAsync(), Times.Once);
        }

        [Fact]
        public async Task CheckIfCreateNewCategoryMethodCreateValidObject()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(new Maybe<Category>());
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<Maybe<Category>>> function = async () =>
                await categoryDomainService.CreateNewCategoryAsync(_categoryId, CategoryName);

            //Assert
            var testResult = await function.Invoke();
            testResult.Value.Id.Should().Be(_categoryId);
            testResult.Value.Name.Should().Be(CategoryName);
            testResult.Value.Description.Should().Be(null);
            testResult.Value.CreatedAt.Should().NotBe(default);
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
            categoryRepository.Verify(cr => cr.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public void CheckIfCreateNewCategoryMethodDoNotThrow()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(new Maybe<Category>());
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<Maybe<Category>>> function = async () =>
                await categoryDomainService.CreateNewCategoryAsync(_categoryId, CategoryName);

            // Assert
            function.Should().NotThrow();
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
        }

        [Fact]
        public void CheckIfCreateNewCategoryMethodThrowWhenCategoryAlreadyExists()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(_category);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<Maybe<Category>>> function = async () =>
                await categoryDomainService.CreateNewCategoryAsync(_categoryId, CategoryName);

            // Assert
            function.Should().Throw<CategoryAlreadyExistsException>()
                .WithMessage($"Unable to add new category, because category with id: {_categoryId} is already exists.");
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
        }

        [Fact]
        public async void CheckIfCreateNewCategoryMethodWithDescriptionCreateValidObjectAndDoNotThrown()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(new Maybe<Category>());
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            var testResult =
                await categoryDomainService.CreateNewCategoryAsync(_categoryId, CategoryName, CategoryDescription);

            //Assert
            testResult.Value.Id.Should().Be(_categoryId);
            testResult.Value.Name.Should().Be(CategoryName);
            testResult.Value.Description.Should().Be(CategoryDescription);
            testResult.Value.CreatedAt.Should().NotBe(default);
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
            categoryRepository.Verify(cr => cr.AddAsync(It.IsAny<Category>()),
                Times.Once);
        }

        [Fact]
        public void CheckIfCreateNewCategoryWithDescriptionMethodDoNotThrow()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(new Maybe<Category>());
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<Maybe<Category>>> function = async () =>
                await categoryDomainService.CreateNewCategoryAsync(_categoryId, CategoryName, CategoryDescription);

            // Assert
            function.Should().NotThrow();
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
        }

        [Fact]
        public void CheckIfCreateNewCategoryWithDescriptionMethodThrowWhenCategoryAlreadyExists()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(_category);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<Maybe<Category>>> function = async () =>
                await categoryDomainService.CreateNewCategoryAsync(_categoryId, CategoryName, CategoryDescription);

            // Assert
            function.Should().Throw<CategoryAlreadyExistsException>()
                .WithMessage($"Unable to add new category, because category with id: {_categoryId} is already exists.");
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
        }

        [Fact]
        public async Task CheckIfChangeCategoryNameMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            var category = new Category(new CategoryId(), CategoryName);
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(category);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            var testResult = await categoryDomainService.SetCategoryNameAsync(_categoryId, "OtherExampleCategoryName");

            //Assert
            testResult.Should().BeTrue();
            category.Name.Should().Be("OtherExampleCategoryName");
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once());
            categoryRepository.Verify(cr => cr.Update(category), Times.Once);
        }


        [Fact]
        public void CheckIfChangeCategoryNameMethodDoNotThrownAnyException()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(_category);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<bool>> action = async () =>
                await categoryDomainService.SetCategoryNameAsync(_categoryId, "OtherExampleCategoryName");

            //Assert
            action.Should().NotThrow();
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
            categoryRepository.Verify(cr => cr.Update(_category), Times.Once);
        }

        [Fact]
        public void CheckIfSetCategoryNameMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync((Category) null);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<bool>> func = async () =>
                await categoryDomainService.SetCategoryNameAsync(_categoryId, "ExampleCategoryName");

            //Assert
            func.Should().Throw<CategoryNotFoundException>()
                .WithMessage($"Unable to mutate category state, because category with id: {_categoryId} not found.");
            categoryRepository.Verify(cr => cr.GetByIdAsync(It.IsAny<CategoryId>()), Times.Once);
            categoryRepository.Verify(cr => cr.Update(null), Times.Never);
        }

        [Fact]
        public async Task CheckIfChangeCategoryDescriptionMethodReturnTrueAndSetValueWhenCorrectValueAreProvided()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            var category = new Category(new CategoryId(), CategoryName);
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(category);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<bool>> func = async () =>
                await categoryDomainService.SetCategoryDescriptionAsync(_categoryId, "OtherExampleCategoryDescription");

            //Assert
            var testResult = await func.Invoke();
            testResult.Should().BeTrue();
            category.Description.Should().Be("OtherExampleCategoryDescription");
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
            categoryRepository.Verify(cr => cr.Update(category), Times.Once);
        }

        [Fact]
        public void CheckIfChangeCategoryDescriptionMethodDoNotThrownAnyException()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(_category);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<bool>> action = async () => await
                categoryDomainService.SetCategoryDescriptionAsync(_categoryId, "OtherExampleCategoryDescription");

            //Assert
            action.Should().NotThrow();
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
            categoryRepository.Verify(cr => cr.Update(_category), Times.Once);
        }

        [Fact]
        public void CheckIfSetCategoryDescriptionMethodThrowExceptionWhenProductHasNoValue()
        {
            // Arrange
            var categoryRepository = new Mock<ICategoryRepository>();
            categoryRepository.Setup(cr => cr.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync((Category) null);
            ICategoryDomainService categoryDomainService = new CategoryDomainService(categoryRepository.Object);

            // Act
            Func<Task<bool>> func = async () =>
                await categoryDomainService.SetCategoryNameAsync(_categoryId, "ExampleCategoryDescription");

            //Assert
            func.Should().Throw<CategoryNotFoundException>()
                .WithMessage($"Unable to mutate category state, because category with id: {_categoryId} not found.");
            categoryRepository.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
            categoryRepository.Verify(cr => cr.Update(null), Times.Never);
        }
    }
}