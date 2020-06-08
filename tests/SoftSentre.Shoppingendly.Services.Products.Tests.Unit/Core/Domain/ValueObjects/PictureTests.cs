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
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Pictures;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Domain.ValueObjects
{
    public class PictureTests
    {
        [Theory]
        [MemberData(nameof(PictureFieldsDataGenerator.IncorrectPictureFields),
            MemberType = typeof(PictureFieldsDataGenerator))]
        public void CheckIfWhenNotEmptyPictureAreCreatingThenCorrectValuesShouldBeProvided(string name, string url)
        {
            // Act
            Action action = () => ProductPicture.Create(name, url);

            //Assert
            action.Should().Throw<InternalException>();
        }

        private class PictureFieldsDataGenerator
        {
            public static IEnumerable<object[]> IncorrectPictureFields =>
                new List<object[]>
                {
                    new object[] {"Name is correct", new string('*', 501)},
                    new object[] {new string('*', 201), "urlIsCorrect"},
                    new object[] {new string('*', 199), "urlIs Correct"},
                    new object[] {new string('*', 201), new string('*', 501)}
                };
        }

        [Fact]
        public void CheckIfIsPossibleToCreateNotEmptyPictureWhenCorrectValuesAreProvided()
        {
            // Arrange
            const string name = "ExamplePictureName";
            const string url = "ExamplePictureUrl";

            // Act
            var picture = ProductPicture.Create(name, url);

            //Assert
            picture.Name.Should().Be(name);
            picture.Url.Should().Be(url);
            picture.IsEmpty.Should().BeFalse();
        }

        [Fact]
        public void CheckIfItPossibleToCreateEmptyPicture()
        {
            // Act
            var picture = ProductPicture.Empty;

            //Assert
            picture.Name.Should().Be(null);
            picture.Url.Should().Be(null);
            picture.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void CheckIfNameIsEmptyThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string url = "ValidUrl";

            // Act
            Action action = () => ProductPicture.Create(string.Empty, url);

            //Assert
            action.Should().Throw<PictureNameCanNotBeEmptyException>()
                .WithMessage("Picture name can not be empty.");
        }

        [Fact]
        public void CheckIfNameIsTooLongThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string url = "ValidUrl";

            // Act
            Action action = () => ProductPicture.Create(new string('*', 201), url);

            //Assert
            action.Should().Throw<PictureNameIsTooLongException>()
                .WithMessage("Picture name can not be longer than 200 characters.");
        }

        [Fact]
        public void CheckIfUrlHaveWhitespacesThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string name = "ValidName";

            // Act
            Action action = () => ProductPicture.Create(name, "Some   url");

            //Assert
            action.Should().Throw<PictureUrlCanNotContainsWhitespacesException>()
                .WithMessage("Picture url can not have whitespaces.");
        }

        [Fact]
        public void CheckIfUrlIsEmptyThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string name = "ValidName";

            // Act
            Action action = () => ProductPicture.Create(name, string.Empty);

            //Assert
            action.Should().Throw<PictureUrlCanNotBeEmptyException>()
                .WithMessage("Picture url can not be empty.");
        }

        [Fact]
        public void CheckIfUrlIsTooLongThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string name = "ValidName";

            // Act
            Action action = () => ProductPicture.Create(name, new string('*', 501));

            //Assert
            action.Should().Throw<PictureUrlIsTooLongException>()
                .WithMessage("Picture url can not be longer than 500 characters.");
        }
    }
}