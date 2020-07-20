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

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Extensions
{
    public class PaginationExtensionMethodsTests
    {
        [Theory]
        [MemberData(nameof(PagedResultTestDataGenerator.EmptyEnumerable),
            MemberType = typeof(PagedResultTestDataGenerator))]
        public void CheckIfPagedResultDoNotThrowExceptionWhenCollectionIsEmpty(
            IEnumerable<object> emptyOrNullEnumerable)
        {
            // Arrange


            // Act
            var testResult = emptyOrNullEnumerable.Paginate();

            // Assert
            testResult.IsEmpty.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(PagedResultTestDataGenerator.EnumerableWithValues),
            MemberType = typeof(PagedResultTestDataGenerator))]
        public void CheckIfPaginateMethodAssignCorrectValues(IEnumerable<int> collection, int page, int resultPerPage,
            int expectedTotalPages)
        {
            // Arrange

            // Act
            var testCollection = collection.ToList();
            var testResult = testCollection.Paginate(page, resultPerPage);

            // Assert
            testResult.IsNotEmpty.Should().BeTrue();
            testResult.CurrentPage.Should().Be(page);
            testResult.ResultsPerPage.Should().Be(resultPerPage);
            testResult.TotalResults.Should().Be(testCollection.Count);
            testResult.TotalPages.Should().Be(expectedTotalPages);
        }

        [Theory]
        [MemberData(nameof(PagedResultTestDataGenerator.EnumerableWithWrongValues),
            MemberType = typeof(PagedResultTestDataGenerator))]
        public void CheckIfPaginateMethodCorrectedInputWhenWrongValuesAreProvided(IEnumerable<int> collection, int page,
            int resultPerPage)
        {
            // Arrange

            // Act
            var testCollection = collection.ToList();
            var testResult = testCollection.Paginate(page, resultPerPage);

            // Assert
            testResult.IsNotEmpty.Should().BeTrue();
            testResult.CurrentPage.Should().Be(1);
            testResult.ResultsPerPage.Should().Be(10);
            testResult.TotalResults.Should().Be(testCollection.Count);
        }

        [Theory]
        [MemberData(nameof(PagedResultTestDataGenerator.EnumerableWithWrongValues),
            MemberType = typeof(PagedResultTestDataGenerator))]
        public void CheckIfLimitMethodCorrectedInputWhenWrongValuesAreProvided(IEnumerable<int> collection, int page,
            int resultPerPage)
        {
            // Arrange

            // Act
            var testCollection = collection.ToList();
            var testResult = testCollection.Limit(page, resultPerPage);

            // Assert
            testResult.Count().Should().Be(10);
        }

        private class PagedResultTestDataGenerator
        {
            public static IEnumerable<object[]> EnumerableWithWrongValues =>
                new List<object[]>
                {
                    new object[] {Enumerable.Range(0, 20), 0, 0},
                    new object[] {Enumerable.Range(0, 50), -1, -10},
                    new object[] {Enumerable.Range(0, 100), -5, -50}
                };

            public static IEnumerable<object[]> EnumerableWithValues =>
                new List<object[]>
                {
                    new object[] {Enumerable.Range(0, 20), 2, 10, 2},
                    new object[] {Enumerable.Range(0, 50), 1, 30, 2},
                    new object[] {Enumerable.Range(0, 100), 4, 10, 10}
                };

            public static IEnumerable<object[]> EmptyEnumerable =>
                new List<object[]>
                {
                    new object[] {new List<object>()},
                    new object[] {null}
                };
        }

        [Fact]
        public void CheckIfPaginateMethodAssignCorrectValueWithoutParameters()
        {
            // Arrange
            var testCollection = Enumerable.Range(0, 50).ToList();

            // Act
            var testResult = testCollection.Paginate();

            // Assert
            testResult.IsNotEmpty.Should().BeTrue();
            testResult.CurrentPage.Should().Be(1);
            testResult.ResultsPerPage.Should().Be(10);
            testResult.TotalResults.Should().Be(testCollection.Count);
            testResult.TotalPages.Should().Be(5);
        }
    }
}