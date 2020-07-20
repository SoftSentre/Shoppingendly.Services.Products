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

using System.Linq;
using AutoMapper;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Application.Mapper;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.AutoMapper;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.AutoMapper.Profiles;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Application.Mappers
{
    public class MapperTests
    {
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

        private readonly IMapperWrapper _mapperWrapper;

        [Fact]
        public void CheckIfItPossibleMapCategoryToBasicCategoryDto()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");

            // Act
            var testResult = _mapperWrapper.MapCategoryToBasicCategoryDto(category);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(category.CategoryId.Id.ToString());
            testResult.Name.Should().Be(category.CategoryName);
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
            testResult.Id.Should().Be(category.CategoryId.Id.ToString());
            testResult.Description.Should().Be(category.CategoryDescription);
            testResult.Name.Should().Be(category.CategoryName);
        }

        [Fact]
        public void CheckIfItPossibleMapCategoryToCategoryWithProductsDto()
        {
            // Arrange
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName",
                Producer.Create("ExampleProducer"));
            product.UploadProductPicture(Picture.Create("name", "picture.jpg"));
            var categorization = new Categorization(product.ProductId, category.CategoryId) {Product = product};
            category.AssignedProducts.Add(categorization);

            // Act
            var testResult = _mapperWrapper.MapCategoryToCategoryWithProductsDto(category);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(category.CategoryId.Id.ToString());
            testResult.Description.Should().Be(category.CategoryDescription);
            testResult.Name.Should().Be(category.CategoryName);
            testResult.Products.Should().HaveCount(1);
            testResult.Products.ToList().Should().Contain(p =>
                p.Id == product.ProductId.Id.ToString() && p.Icon.Url == product.ProductPicture.Url &&
                p.Name == product.ProductName &&
                p.Producer == product.ProductProducer.Name);
        }

        [Fact]
        public void CheckIfItPossibleMapCreatorToBasicCreatorDto()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            var testResult = _mapperWrapper.MapCreatorToBasicCreatorDto(creator);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(creator.CreatorId.Id.ToString());
            testResult.Name.Should().Be(creator.CreatorName);
        }

        [Fact]
        public void CheckIfItPossibleMapCreatorToCreatorDto()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);

            // Act
            var testResult = _mapperWrapper.MapCreatorToCreatorDto(creator);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(creator.CreatorId.Id.ToString());
            testResult.Name.Should().Be(creator.CreatorName);
            testResult.Role.Id.Should().Be(creator.CreatorRole.Id.ToString());
            testResult.Role.Role.Should().Be(creator.CreatorRole.Name);
        }

        [Fact]
        public void CheckIfItPossibleMapCreatorToCreatorWithProductsDto()
        {
            // Arrange
            var creator = new Creator(new CreatorId(), "Creator", CreatorRole.Admin);
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName",
                Producer.Create("ExampleProducer"));
            var picture = Picture.Create("Example", "picture.jpg");
            product.UploadProductPicture(picture);
            creator.Products.Add(product);

            // Act
            var testResult = _mapperWrapper.MapCreatorToCreatorWithProductsDto(creator);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(creator.CreatorId.Id.ToString());
            testResult.Name.Should().Be(creator.CreatorName);
            testResult.Role.Id.Should().Be(creator.CreatorRole.Id.ToString());
            testResult.Role.Role.Should().Be(creator.CreatorRole.Name);
            testResult.Products.Items.Should().Contain(p =>
                p.Id == product.ProductId.Id.ToString() && p.Name == product.ProductName &&
                p.Producer == product.ProductProducer.Name &&
                p.Icon.Url == product.ProductPicture.Url);
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
        public void CheckIfItPossibleMapProductToProductDetailsDto()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName",
                Producer.Create("ExampleProducer"));
            var picture = Picture.Create("Name", "picture.jpg");
            product.UploadProductPicture(picture);
            var category = new Category(new CategoryId(), "ExampleCategory", "Other correct description");
            var secondCategory = new Category(new CategoryId(), "SecondExampleCategory", "Other correct description");
            var assignedCategory = Categorization.Create(product.ProductId, category.CategoryId);
            assignedCategory.Category = category;
            var secondAssignedCategory = Categorization.Create(product.ProductId, secondCategory.CategoryId);
            secondAssignedCategory.Category = secondCategory;
            product.AssignedCategories.Add(assignedCategory);
            product.AssignedCategories.Add(secondAssignedCategory);

            // Act
            var testResult = _mapperWrapper.MapProductToProductDetailsDto(product);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(product.ProductId.Id.ToString());
            testResult.Name.Should().Be(product.ProductName);
            testResult.Producer.Should().Be(product.ProductProducer.Name);
            testResult.Picture.Url.Should().Be(product.ProductPicture.Url);
            testResult.Categories.Should().Contain(c => c == category.CategoryName);
            testResult.Categories.Should().Contain(c => c == secondCategory.CategoryName);
        }

        [Fact]
        public void CheckIfItPossibleMapProductToProductDto()
        {
            // Arrange
            var product = new Product(new ProductId(), new CreatorId(), "OtherExampleProductName",
                Producer.Create("ExampleProducer"));
            var picture = Picture.Create("Name", "picture.jpg");
            product.UploadProductPicture(picture);

            // Act
            var testResult = _mapperWrapper.MapProductToProductDto(product);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(product.ProductId.Id.ToString());
            testResult.Name.Should().Be(product.ProductName);
            testResult.Producer.Should().Be(product.ProductProducer.Name);
            testResult.Icon.Url.Should().Be(product.ProductPicture.Url);
        }

        [Fact]
        public void CheckIfItPossibleMapRoleToRoleDto()
        {
            // Arrange
            var role = CreatorRole.Moderator;

            // Act
            var testResult = _mapperWrapper.MapRoleToRoleDto(role);

            // Assert
            testResult.Should().NotBeNull();
            testResult.Id.Should().Be(role.Id.ToString());
            testResult.Role.Should().Be(role.Name);
        }
    }
}