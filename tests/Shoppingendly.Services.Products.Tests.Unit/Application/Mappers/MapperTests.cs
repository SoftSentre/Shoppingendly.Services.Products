using System.Linq;
using AutoMapper;
using FluentAssertions;
using Shoppingendly.Services.Products.Application.Mapper;
using Shoppingendly.Services.Products.Application.Mapper.Base;
using Shoppingendly.Services.Products.Application.Mapper.Profiles;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Application.Mappers
{
    public class MapperTests
    {
        private readonly IMapperWrapper _mapperWrapper;

        public MapperTests()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile(new CategoryProfile());
                    cfg.AddProfile(new CreatorProfile());
                    cfg.AddProfile(new PictureProfile());
                    cfg.AddProfile(new ProductProfile());
                    cfg.AddProfile(new RoleProfile());
                });

            var testMapper = config.CreateMapper();
            _mapperWrapper = new MapperWrapper(testMapper);
        }

        [Fact]
        public void CheckIfItPossibleMapCategoryToBasicCategoryDto()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");

            // Act
            var testResult = _mapperWrapper.MapCategoryToBasicCategoryDto(category);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(category.Id.Id.ToString());
            testResult.Name.Should().Be(category.Name);
        }

        [Fact]
        public void CheckIfItPossibleMapCategoryToCategoryDto()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");

            // Act
            var testResult = _mapperWrapper.MapCategoryToCategoryDto(category);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(category.Id.Id.ToString());
            testResult.Description.Should().Be(category.Description);
            testResult.Name.Should().Be(category.Name);
        }

        [Fact]
        public void CheckIfItPossibleMapCategoryToCategoryWithProductsDto()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName", "ExampleProducer");
            product.AddOrChangePicture(new Picture("name", "picture.jpg"));
            var productCategory = new ProductCategory(product.Id, category.Id) {Product = product};
            category.ProductCategories.Add(productCategory);

            // Act
            var testResult = _mapperWrapper.MapCategoryToCategoryWithProductsDto(category);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(category.Id.Id.ToString());
            testResult.Description.Should().Be(category.Description);
            testResult.Name.Should().Be(category.Name);
            testResult.Products.Should().HaveCount(1);
            testResult.Products.ToList().Should().Contain(p =>
                p.Id == product.Id.Id.ToString() && p.Icon.Url == product.Picture.Url && p.Name == product.Name &&
                p.Producer == product.Producer);
        }

        [Fact]
        public void CheckIfItPossibleMapCreatorToBasicCreatorDto()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            var testResult = _mapperWrapper.MapCreatorToBasicCreatorDto(creator);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(creator.Id.Id.ToString());
            testResult.Name.Should().Be(creator.Name);
        }

        [Fact]
        public void CheckIfItPossibleMapCreatorToCreatorDto()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);

            // Act
            var testResult = _mapperWrapper.MapCreatorToCreatorDto(creator);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(creator.Id.Id.ToString());
            testResult.Name.Should().Be(creator.Name);
            testResult.Role.Id.Should().Be(creator.Role.Id.ToString());
            testResult.Role.Role.Should().Be(creator.Role.Name);
        }

        [Fact]
        public void CheckIfItPossibleMapCreatorToCreatorWithProductsDto()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", "creator@email.com", Role.Admin);
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName", "ExampleProducer");
            var picture = new Picture("Example", "picture.jpg");
            product.AddOrChangePicture(picture);
            creator.Products.Add(product);

            // Act
            var testResult = _mapperWrapper.MapCreatorToCreatorWithProductsDto(creator);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(creator.Id.Id.ToString());
            testResult.Name.Should().Be(creator.Name);
            testResult.Role.Id.Should().Be(creator.Role.Id.ToString());
            testResult.Role.Role.Should().Be(creator.Role.Name);
            testResult.Products.Should().Contain(p =>
                p.Id == product.Id.Id.ToString() && p.Name == product.Name && p.Producer == product.Producer &&
                p.Icon.Url == product.Picture.Url);
        }

        [Fact]
        public void CheckIfItPossibleMapRoleToRoleDto()
        {
            // Arrange
            var role = Role.Moderator;

            // Act
            var testResult = _mapperWrapper.MapRoleToRoleDto(role);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(role.Id.ToString());
            testResult.Role.Should().Be(role.Name);
        }

        [Fact]
        public void CheckIfItPossibleMapPictureToPictureDto()
        {
            // Arrange
            var picture = Picture.Create("Name", "picture.jpg");

            // Act
            var testResult = _mapperWrapper.MapPictureToPictureDto(picture);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Url.Should().Be(picture.Url);
        }

        [Fact]
        public void CheckIfItPossibleMapProductToProductDto()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName", "ExampleProducer");
            var picture = Picture.Create("Name", "picture.jpg");
            product.AddOrChangePicture(picture);

            // Act
            var testResult = _mapperWrapper.MapProductToProductDto(product);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(product.Id.Id.ToString());
            testResult.Name.Should().Be(product.Name);
            testResult.Producer.Should().Be(product.Producer);
            testResult.Icon.Url.Should().Be(product.Picture.Url);
        }

        [Fact]
        public void CheckIfItPossibleMapProductToProductDetailsDto()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName", "ExampleProducer");
            var picture = Picture.Create("Name", "picture.jpg");
            product.AddOrChangePicture(picture);
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");
            var secondCategory = new Category(new CategoryId(), "SecondExampleCategory", "Other correct description");
            var productCategory = ProductCategory.Create(product.Id, category.Id);
            productCategory.Category = category;
            var secondProductCategory = ProductCategory.Create(product.Id, secondCategory.Id);
            secondProductCategory.Category = secondCategory;
            product.ProductCategories.Add(productCategory);
            product.ProductCategories.Add(secondProductCategory);

            // Act
            var testResult = _mapperWrapper.MapProductToProductDetailsDto(product);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(product.Id.Id.ToString());
            testResult.Name.Should().Be(product.Name);
            testResult.Producer.Should().Be(product.Producer);
            testResult.Picture.Url.Should().Be(product.Picture.Url);
            testResult.Categories.Should().Contain(c => c == category.Name);
            testResult.Categories.Should().Contain(c => c == secondCategory.Name);
        }
    }
}