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
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.ValueObjects
{
    public class ProductProducerTests
    {
        [Fact]
        public void CheckIfCreateProductProducerMethodWorkingProperlyWhenCorrectValueHasBeenProvided()
        {
            // Arrange
            const string producerName = "ExampleProducerName";
            
            // Act
            var productProducer = ProductProducer.CreateProductProducer("ExampleProducerName");

            // Assert
            productProducer.Name.Should().BeEquivalentTo(producerName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CheckIfCreateProductProducerMethodThrowProperExceptionAndMessageWhenEmptyNameHasBeenProvided(
            string name)
        {
            // Arrange
            
            // Act
            Action action = () => ProductProducer.CreateProductProducer(name);

            // Assert
            action.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be empty.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenTooLongNameHasBeenProvided()
        {
            // Arrange

            // Act
            Action action = () =>             ProductProducer.CreateProductProducer("IProvideMaximalNumberOfLettersAndFewMoreBecauseProducerCanNotBeLongerThan50Characters");
            

            // Assert
            action.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be longer than 50 characters.");
        }

        [Fact]
        public void CheckIfSetProductProducerMethodThrowProperExceptionAndMessageWhenTooShortNameHasBeenProvided()
        {
            // Arrange
            
            // Act
            Action action = () => ProductProducer.CreateProductProducer("p");

            // Assert
            action.Should().Throw<InvalidProductProducerException>()
                .WithMessage("Product producer can not be shorter than 2 characters.");
        }
    }
}