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
using System.Threading.Tasks;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Services
{
    public class ProductBusinessRulesCheckerTests : IAsyncLifetime
    {
        private IProductBusinessRulesChecker _productBusinessRulesChecker;

        private ProductId _productId;
        private Picture _productPicture;
        private ProductProducer _productProducer;
        private string _productName;

        public static IEnumerable<object[]> InvalidPictures =>
            new[]
            {
                new object[] {null},
                new object[] {Picture.Empty}
            };

        public async Task InitializeAsync()
        {
            _productBusinessRulesChecker = new ProductBusinessRulesChecker();
            _productId = new ProductId(new Guid("9AC9CAA7-1629-4BA6-9BB2-797A073ACD3B"));
            _productPicture = Picture.Create("examplePictureName", "examplePictureUrl");
            _productProducer = ProductProducer.Create("exampleProductProducer");
            _productName = "exampleProductName";

            await Task.CompletedTask;
        }

        [Fact]
        public void FalseWhenProductIdCanNotBeEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var productIdCanNotBeEmptyRuleIsBroken =
                _productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(_productId);

            // Assert
            productIdCanNotBeEmptyRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenProductIdCanNotBeEmptyRuleIsBroken()
        {
            // Arrange
            _productId = new ProductId(Guid.Empty);

            // Act
            var productIdCanNotBeEmptyRuleIsBroken =
                _productBusinessRulesChecker.ProductIdCanNotBeEmptyRuleIsBroken(_productId);

            // Assert
            productIdCanNotBeEmptyRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenProductNameCanBeNotNullOrEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var productNameCanNotBeNullOrEmptyRuleIsBroken =
                _productBusinessRulesChecker.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_productName);

            // Assert
            productNameCanNotBeNullOrEmptyRuleIsBroken.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void TrueWhenProductNameCanBeNotNullOrEmptyRuleIsBroken(string productName)
        {
            // Arrange
            _productName = productName;

            // Act
            var productNameCanNotBeNullOrEmptyRuleIsBroken =
                _productBusinessRulesChecker.ProductNameCanNotBeNullOrEmptyRuleIsBroken(_productName);

            // Assert
            productNameCanNotBeNullOrEmptyRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenProductNameCanNotBeShorterThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var productNameCanNotBeShorterThanRuleIsBroken =
                _productBusinessRulesChecker.ProductNameCanNotBeShorterThanRuleIsBroken(_productName);

            // Assert
            productNameCanNotBeShorterThanRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenProductNameCanNotBeShorterThanRuleIsBroken()
        {
            // Arrange
            _productName = new string('a', GlobalValidationVariables.ProductNameMinLength - 1);

            // Act
            var productNameCanNotBeShorterThanRuleIsBroken =
                _productBusinessRulesChecker.ProductNameCanNotBeShorterThanRuleIsBroken(_productName);

            // Assert
            productNameCanNotBeShorterThanRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenProductNameCanNotBeLongerThanRuleIsNotBroken()
        {
            // Arrange

            // Act
            var productNameCanNotBeLongerThanRuleIsBroken =
                _productBusinessRulesChecker.ProductNameCanNotBeLongerThanRuleIsBroken(_productName);

            // Assert
            productNameCanNotBeLongerThanRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenProductNameCanNotBeLongerThanRuleIsBroken()
        {
            // Arrange
            _productName = new string('a', GlobalValidationVariables.ProductNameMaxLength + 1);

            // Act
            var productNameCanNotBeLongerThanRuleIsBroken =
                _productBusinessRulesChecker.ProductNameCanNotBeLongerThanRuleIsBroken(_productName);

            // Assert
            productNameCanNotBeLongerThanRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenProductProducerCanBeNullRuleIsNotBroken()
        {
            // Arrange

            // Act
            var productProducerCanNotBeNullRuleIsBroken =
                _productBusinessRulesChecker.ProductProducerCanNotBeNullRuleIsBroken(_productProducer);

            // Assert
            productProducerCanNotBeNullRuleIsBroken.Should().BeFalse();
        }

        [Fact]
        public void TrueWhenProductProducerCanBeNullRuleIsBroken()
        {
            // Arrange
            _productProducer = null;

            // Act
            var productProducerCanNotBeNullRuleIsBroken =
                _productBusinessRulesChecker.ProductProducerCanNotBeNullRuleIsBroken(_productProducer);

            // Assert
            productProducerCanNotBeNullRuleIsBroken.Should().BeTrue();
        }

        [Fact]
        public void FalseWhenProductPictureCanNotBeNullOrEmptyRuleIsNotBroken()
        {
            // Arrange

            // Act
            var productPictureCanNotBeNullOrEmptyRuleIsBroken =
                _productBusinessRulesChecker.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture);

            // Assert
            productPictureCanNotBeNullOrEmptyRuleIsBroken.Should().BeFalse();
        }

        [Theory, MemberData(nameof(InvalidPictures))]
        public void TrueWhenProductPictureCanNotBeNullOrEmptyRuleIsBroken(Picture productPicture)
        {
            // Arrange
            _productPicture = productPicture;

            // Act
            var productPictureCanNotBeNullOrEmptyRuleIsBroken =
                _productBusinessRulesChecker.ProductPictureCanNotBeNullOrEmptyRuleIsBroken(_productPicture);

            // Assert
            productPictureCanNotBeNullOrEmptyRuleIsBroken.Should().BeTrue();
        }

        public async Task DisposeAsync()
        {
            _productBusinessRulesChecker = null;
            _productId = null;
            _productPicture = null;
            _productProducer = null;
            _productId = null;

            await Task.CompletedTask;
        }
    }
}