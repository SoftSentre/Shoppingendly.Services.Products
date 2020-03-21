using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Extensions
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void CheckIfIsEmptyMethodForEnumerableReturnFalseWhenCollectionContainElements()
        {
            // Arrange
            var collection = Enumerable.Range(1, 5);

            // Act
            var testResult = collection.IsEmpty();

            // Assert
            testResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfIsNotEmptyMethodForEnumerableReturnTrueWhenCollectionContainElements()
        {
            // Arrange
            var collection = Enumerable.Range(1, 5);

            // Act
            var testResult = collection.IsNotEmpty();

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfIsEmptyMethodForListReturnFalseWhenCollectionContainElements()
        {
            // Arrange
            var collection = new List<int> {1, 2, 3};

            // Act
            var testResult = collection.IsEmpty();

            // Assert
            testResult.Should().BeFalse();
        }
        
        [Fact]
        public void CheckIfIsNotEmptyMethodForListReturnTrueWhenCollectionContainElements()
        {
            // Arrange
            var collection = new List<int> {1, 2, 3};

            // Act
            var testResult = collection.IsNotEmpty();

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfIsEmptyMethodForEnumerableReturnTrueWhenCollectionIsEmpty()
        {
            // Arrange
            var collection = Enumerable.Empty<int>();

            // Act
            var testResult = collection.IsEmpty();

            // Assert
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfIsNotEmptyMethodForEnumerableReturnFalseWhenCollectionIsEmpty()
        {
            // Arrange
            var collection = Enumerable.Empty<int>();
            IEnumerable<int> nullCollection = null;

            // Act
            var testResult = collection.IsNotEmpty();
            var secondTestResult = nullCollection.IsNotEmpty();

            // Assert
            testResult.Should().BeFalse();
            secondTestResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfIsEmptyMethodForListReturnTrueWhenCollectionIsEmpty()
        {
            // Arrange
            var collection = new List<int>();

            // Act
            var testResult = collection.IsEmpty();

            // Assert
            testResult.Should().BeTrue();
        }
        
        [Fact]
        public void CheckIfIsNotEmptyMethodForListReturnFalseWhenCollectionIsEmpty()
        {
            // Arrange
            var collection = new List<int>();
            List<int> nullCollection = null;

            // Act
            var testResult = collection.IsNotEmpty();
            var secondTestResult = nullCollection.IsNotEmpty();

            // Assert
            testResult.Should().BeFalse();
            secondTestResult.Should().BeFalse();
        }
    }
}