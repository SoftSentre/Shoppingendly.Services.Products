using System;
using System.Collections.Generic;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions;
using Shoppingendly.Services.Products.Core.Exceptions.Products;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Domain.ValueObjects
{
    public class PictureTests 
    {
        [Fact]
        public void CheckIfIsPossibleToCreateNotEmptyPictureWhenCorrectValuesAreProvided()
        {
            // Arrange
            const string name = "ExamplePictureName";
            const string url = "ExamplePictureUrl";

            // Act
            var picture = Picture.Create(name, url);

            //Assert
            picture.Name.Should().Be(name);
            picture.Url.Should().Be(url);
            picture.IsEmpty.Should().BeFalse();
        }

        [Fact]
        public void CheckIfItPossibleToCreateEmptyPicture()
        {
            // Act
            var picture = Picture.Empty;

            //Assert
            picture.Name.Should().Be(null);
            picture.Url.Should().Be(null);
            picture.IsEmpty.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(PictureFieldsDataGenerator.IncorrectPictureFields),
            MemberType = typeof(PictureFieldsDataGenerator))]
        public void CheckIfWhenNotEmptyPictureAreCreatingThenCorrectValuesShouldBeProvided(string name, string url)
        {
            // Act
            Action action = () => Picture.Create(name, url);

            //Assert
            action.Should().Throw<ShoppingendlyException>();
        }

        [Fact]
        public void CheckIfNameIsEmptyThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string url = "ValidUrl";

            // Act
            Action action = () => Picture.Create(string.Empty, url);

            //Assert
            action.Should().Throw<InvalidPictureNameException>()
                .WithMessage("Picture name can not be empty.");
        }

        [Fact]
        public void CheckIfNameIsTooLongThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string url = "ValidUrl";

            // Act
            Action action = () => Picture.Create(new string('*', 201), url);

            //Assert
            action.Should().Throw<InvalidPictureNameException>()
                .WithMessage("Picture name can not be longer than 200 characters.");
        }

        [Fact]
        public void CheckIfUrlIsEmptyThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string name = "ValidName";

            // Act
            Action action = () => Picture.Create(name, string.Empty);

            //Assert
            action.Should().Throw<InvalidPictureUrlException>()
                .WithMessage("Picture url can not be empty.");
        }

        [Fact]
        public void CheckIfUrlIsTooLongThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string name = "ValidName";

            // Act
            Action action = () => Picture.Create(name, new string('*', 501));

            //Assert
            action.Should().Throw<InvalidPictureUrlException>()
                .WithMessage("Picture url can not be longer than 500 characters.");
        }

        [Fact]
        public void CheckIfUrlHaveWhitespacesThenExceptionWasThrownAndAppropriateMessageHasBeenWritten()
        {
            // Arrange
            const string name = "ValidName";

            // Act
            Action action = () => Picture.Create(name, "Some   url");

            //Assert
            action.Should().Throw<InvalidPictureUrlException>()
                .WithMessage("Picture url can not have whitespaces.");
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
    }
}