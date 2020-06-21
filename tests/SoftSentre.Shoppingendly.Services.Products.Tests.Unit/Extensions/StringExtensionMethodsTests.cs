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

using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Extensions
{
    public class StringExtensionMethodsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CheckIfIsEmptyMethodReturnTrueIfValueIsNullOrEmpty(string value)
        {
            // Arrange 
            var stringValue = value;

            // Act
            var isEmpty = stringValue.IsEmpty();

            // Assert
            isEmpty.Should().BeTrue();
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        public void CheckIfCreateStringWithSpecificNumberOfCharactersMethodWorkingProperly(int numberOfCharacters)
        {
            // act
            var testResult = StringExtensionMethods.CreateStringWithSpecificNumberOfCharacters(numberOfCharacters);

            // assert
            testResult.Should().NotBeEmpty();
            testResult.Should().HaveLength(numberOfCharacters);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CheckIfIsNotEmptyMethodReturnFalseIfValueIsNullOrEmpty(string value)
        {
            // Arrange 
            var stringValue = value;

            // Act
            var isEmpty = stringValue.IsNotEmpty();

            // Assert
            isEmpty.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CheckIfOrEmptyMethodReturnEmptyStringAlwaysWhenInputIsNullOrEmpty(string value)
        {
            // Arrange 
            var stringValue = value;

            // Act
            var orEmpty = stringValue.OrEmpty();

            // Assert
            orEmpty.Should().Be(string.Empty);
        }

        [Theory]
        [InlineData(null, "value")]
        [InlineData("value", null)]
        [InlineData("", "value")]
        [InlineData("value", "")]
        public void CheckIfEqualsCaseInvariantMethodReturnFalseWhenInputValuesAreNullOrEmpty(string value,
            string valueToCompare)
        {
            // Arrange 
            var inputValue = value;
            var inputValueToCompare = valueToCompare;

            // Act
            var isTheSame = inputValue.EqualsCaseInvariant(inputValueToCompare);

            // Assert
            isTheSame.Should().BeFalse();
        }

        [Fact]
        public void CheckIfAggregateLinesMethodReturnCorrectStringWhenInputIsValid()
        {
            // Arrange 
            var inputValue = new[] {"Convert", "This", "String", "To", "Underscore"};

            // Act
            var aggregatedLines = inputValue.AggregateLines();

            // Assert
            aggregatedLines.Should().Be("Convert\nThis\nString\nTo\nUnderscore");
        }

        [Fact]
        public void CheckIfEqualsCaseInvariantReturnTrueWhenInputValuesAreTheSame()
        {
            // Arrange 
            const string inputValue = "TheSameValue";
            const string inputValueToCompare = "TheSameValue";

            // Act
            var isTheSame = inputValue.EqualsCaseInvariant(inputValueToCompare);

            // Assert
            isTheSame.Should().BeTrue();
        }

        [Fact]
        public void CheckIfIsEmptyMethodReturnFalseIfValueIsNotNullOrNotEmpty()
        {
            // Arrange
            const string stringValue = "ExampleString";

            // Act
            var isEmpty = stringValue.IsEmpty();

            //Assert
            isEmpty.Should().BeFalse();
        }

        [Fact]
        public void CheckIfIsLongerThanMethodReturnFalseWhenValueIsShorterThanInput()
        {
            // Arrange 
            const string stringValue = "ExampleString";

            // Act
            var isLonger = stringValue.IsLongerThan(15);

            // Assert
            isLonger.Should().BeFalse();
        }

        [Fact]
        public void CheckIfIsLongerThanMethodReturnTrueWhenValueIsLongerThanInput()
        {
            // Arrange 
            const string stringValue = "ExampleString";

            // Act
            var isLonger = stringValue.IsLongerThan(12);

            // Assert
            isLonger.Should().BeTrue();
        }

        [Fact]
        public void CheckIfIsNotEmptyMethodReturnTrueIfValueIsNotNullOrNotEmpty()
        {
            // Arrange
            const string stringValue = "ExampleString";

            // Act
            var isEmpty = stringValue.IsNotEmpty();

            //Assert
            isEmpty.Should().BeTrue();
        }

        [Fact]
        public void CheckIfIsShorterThanMethodReturnFalseWhenValueIsLongerThanInput()
        {
            // Arrange 
            const string stringValue = "ExampleString";

            // Act
            var isLonger = stringValue.IsShorterThan(12);

            // Assert
            isLonger.Should().BeFalse();
        }

        [Fact]
        public void CheckIfIsShorterThanMethodReturnTrueWhenValueIsShorterThanInput()
        {
            // Arrange 
            const string stringValue = "ExampleString";

            // Act
            var isLonger = stringValue.IsShorterThan(15);

            // Assert
            isLonger.Should().BeTrue();
        }

        [Fact]
        public void CheckIfOrEmptyMethodReturnValueAlwaysWhenInputIsNotNullOrNotEmpty()
        {
            // Arrange 
            const string stringValue = "ExampleString";

            // Act
            var orEmpty = stringValue.OrEmpty();

            // Assert
            orEmpty.Should().Be(stringValue);
        }

        [Fact]
        public void CheckIfTrimToLowerMethodEliminateWhiteSpacesAndChangeTheWordToLower()
        {
            // Arrange
            const string stringValue = "   ExampleString  ";

            // Act
            var trimmedAndLoweredString = stringValue.TrimToLower();

            //Assert
            trimmedAndLoweredString.Should().Be("examplestring");
        }

        [Fact]
        public void CheckIfTrimToUpperMethodEliminateWhiteSpacesAndChangeTheWordToUpper()
        {
            // Arrange
            const string stringValue = "   ExampleString  ";

            // Act
            var trimmedAndUpperString = stringValue.TrimToUpper();

            //Assert
            trimmedAndUpperString.Should().Be("EXAMPLESTRING");
        }

        [Fact]
        public void CheckIfUnderscoreMethodReturnCorrectStringValue()
        {
            // Arrange 
            const string inputValue = "ConvertThisStringToUnderscore";

            // Act
            var underscoredString = inputValue.Underscore();

            // Assert
            underscoredString.Should().Be("Convert_This_String_To_Underscore");
        }
    }
}