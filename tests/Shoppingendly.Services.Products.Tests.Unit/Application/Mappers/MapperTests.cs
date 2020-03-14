using System.Linq;
using AutoMapper;
using FluentAssertions;
using Shoppingendly.Services.Products.Application.DTO;
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
            product.AddOrChangePicture(new Picture("name", "eeee.jpg"));
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
    }
}