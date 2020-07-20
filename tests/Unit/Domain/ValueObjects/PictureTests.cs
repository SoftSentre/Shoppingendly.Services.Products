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
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Pictures;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Globals;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.ValueObjects
{
    public class PictureTests : IAsyncLifetime
    {
        private string _pictureName;
        private string _pictureUrl;

        public async Task InitializeAsync()
        {
            _pictureName = "ExamplePictureName";
            _pictureUrl = "ExamplePictureUrl";

            await Task.CompletedTask;
        }

        public static IEnumerable<object[]> IncorrectPictureFields =>
            new List<object[]>
            {
                new object[] {"Name is correct", new string('*', 501)},
                new object[] {new string('*', 201), "urlIsCorrect"},
                new object[] {new string('*', 199), "urlIs Correct"},
                new object[] {new string('*', 201), new string('*', 501)}
            };

        [Theory]
        [MemberData(nameof(IncorrectPictureFields))]
        public void FailToCreateAPictureWhenNameOrUrlAreInvalid(string name, string url)
        {
            // Act
            Action action = () => Picture.Create(name, url);

            //Assert
            action.Should().Throw<InternalException>();
        }

        public async Task DisposeAsync()
        {
            _pictureName = null;
            _pictureUrl = null;

            await Task.CompletedTask;
        }

        [Fact]
        public void AppropriateExceptionHasBeenThrownWhenPictureNameIsEmpty()
        {
            // Arrange

            // Act
            Action action = () => Picture.Create(string.Empty, _pictureUrl);

            //Assert
            action.Should().Throw<PictureNameCanNotBeEmptyException>()
                .Where(e => e.Code == ErrorCodes.PictureNameCanNotBeEmpty)
                .WithMessage("Picture name can not be empty.");
        }

        [Fact]
        public void AppropriateExceptionHasBeenThrownWhenPictureNameIsTooLong()
        {
            // Arrange

            // Act
            Action action = () => Picture.Create(new string('*', 201), _pictureUrl);

            //Assert
            action.Should().Throw<PictureNameIsTooLongException>()
                .Where(e => e.Code == ErrorCodes.PictureNameIsTooLong)
                .WithMessage("Picture name can not be longer than 200 characters.");
        }

        [Fact]
        public void AppropriateExceptionHasBeenThrownWhenPictureUrlHaveAWhitespaces()
        {
            // Arrange

            // Act
            Action action = () => Picture.Create(_pictureName, "Some   url");

            //Assert
            action.Should().Throw<PictureUrlCanNotContainsWhitespacesException>()
                .Where(e => e.Code == ErrorCodes.PictureUrlCanNotContainsWhitespaces)
                .WithMessage("Picture url can not have whitespaces.");
        }

        [Fact]
        public void AppropriateExceptionHasBeenThrownWhenPictureUrlIsEmpty()
        {
            // Arrange

            // Act
            Action action = () => Picture.Create(_pictureName, string.Empty);

            //Assert
            action.Should().Throw<PictureUrlCanNotBeEmptyException>()
                .Where(e => e.Code == ErrorCodes.PictureUrlCanNotBeEmpty)
                .WithMessage("Picture url can not be empty.");
        }

        [Fact]
        public void AppropriateExceptionHasBeenThrownWhenPictureUrlIsTooLong()
        {
            // Arrange

            // Act
            Action action = () => Picture.Create(_pictureName, new string('*', 501));

            //Assert
            action.Should().Throw<PictureUrlIsTooLongException>()
                .Where(e => e.Code == ErrorCodes.PictureUrlIsTooLong)
                .WithMessage("Picture url can not be longer than 500 characters.");
        }

        [Fact]
        public void GetHashCodeShouldMakeCorrectComparison()
        {
            // Arrange
            var picture = Picture.Create("exampleName", "exampleUrl");
            var theSamePicture = Picture.Create("exampleName", "exampleUrl");
            var otherPicture = Picture.Create("someName", "someUrl");

            // Act
            var theSame = picture.GetHashCode() == theSamePicture.GetHashCode();
            var notTheSame = picture.GetHashCode() == otherPicture.GetHashCode();

            // Assert
            theSame.Should().BeTrue();
            notTheSame.Should().BeFalse();
        }

        [Fact]
        public void IsPossibleToCreateEmptyPicture()
        {
            // Act
            var picture = Picture.Empty;

            //Assert
            picture.Name.Should().Be(null);
            picture.Url.Should().Be(null);
            picture.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void SuccessToCreateAPictureWhenNameAndUrlAreCorrect()
        {
            // Arrange

            // Act
            var picture = Picture.Create(_pictureName, _pictureUrl);

            //Assert
            picture.Name.Should().Be(_pictureName);
            picture.Url.Should().Be(_pictureUrl);
            picture.IsEmpty.Should().BeFalse();
        }
    }
}