using System;
using System.Collections.Generic;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions;
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

        private class PictureFieldsDataGenerator
        {
            public static IEnumerable<object[]> IncorrectPictureFields =>
                new List<object[]>
                {
                    new object[] {"Name is correct", new string('*', 501)},
                    new object[] {new string('*', 201), "urlIsCorrect"},
                    new object[] {new string('*', 201), new string('*', 501)}
                };
        }
    }
}