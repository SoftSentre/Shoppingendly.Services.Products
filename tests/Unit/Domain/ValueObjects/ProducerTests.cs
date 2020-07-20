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
using System.Threading.Tasks;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Producers;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.ValueObjects
{
    public class ProducerTests : IAsyncLifetime
    {
        private string _producerName;

        public async Task InitializeAsync()
        {
            _producerName = "ExampleProducerName";

            await Task.CompletedTask;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AppropriateExceptionHasBeenThrownWhenProductProducerIsEmpty(string producerName)
        {
            // Arrange

            // Act
            Action action = () => Producer.Create(producerName);

            // Assert
            action.Should().Throw<ProductProducerNameCanNotBeEmptyException>()
                .Where(e => e.Code == ErrorCodes.ProductProducerNameCanNotBeEmpty)
                .WithMessage("Product producer can not be empty.");
        }

        public async Task DisposeAsync()
        {
            _producerName = null;

            await Task.CompletedTask;
        }

        [Fact]
        public void AppropriateExceptionHasBeenThrownWhenProductProducerIsTooLong()
        {
            // Arrange
            _producerName = new string('a', GlobalValidationVariables.ProductProducerMaxLength + 1);

            // Act
            Action action = () => Producer.Create(_producerName);

            // Assert
            action.Should().Throw<ProductProducerNameIsTooLongException>()
                .Where(e => e.Code == ErrorCodes.ProductProducerNameIsTooLong)
                .WithMessage("Product producer can not be longer than 50 characters.");
        }

        [Fact]
        public void AppropriateExceptionHasBeenThrownWhenProductProducerIsTooShort()
        {
            // Arrange
            _producerName = new string('a', GlobalValidationVariables.ProductProducerMinLength - 1);

            // Act
            Action action = () => Producer.Create(_producerName);

            // Assert
            action.Should().Throw<ProductProducerNameIsTooShortException>()
                .Where(e => e.Code == ErrorCodes.ProductProducerNameIsTooShort)
                .WithMessage("Product producer can not be shorter than 2 characters.");
        }

        [Fact]
        public void SuccessToCreateProducerWhenNameIsCorrect()
        {
            // Arrange

            // Act
            var productProducer = Producer.Create(_producerName);

            // Assert
            productProducer.Name.Should().BeEquivalentTo(_producerName);
        }
    }
}