using System.Collections.Generic;
using FluentAssertions;
using Shoppingendly.Services.Products.Core.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Core.Extensions
{
    public class CollectionExtensionsTests
    {
        [Theory]
        [MemberData(nameof(CollectionExtensionsTestsDataGenerator.EmptyLists),
            MemberType = typeof(CollectionExtensionsTestsDataGenerator))]
        public void CheckIfIsEmptyOrIsNotEmptyMethodReturnValidValuesWhenListIsEmpty(List<object> emptyOrNullList)
        {
            var testResult = emptyOrNullList.IsEmpty();
            testResult.Should().BeTrue();

            testResult = emptyOrNullList.IsNotEmpty();
            testResult.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(CollectionExtensionsTestsDataGenerator.EmptyEnumerable),
            MemberType = typeof(CollectionExtensionsTestsDataGenerator))]
        public void CheckIfIsEmptyMethodReturnValidValuesWhenEnumerableIsEmpty(
            IEnumerable<object> emptyOrNullEnumerable)
        {
            var testResult = emptyOrNullEnumerable.IsEmpty();
            testResult.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(CollectionExtensionsTestsDataGenerator.EmptyEnumerable),
            MemberType = typeof(CollectionExtensionsTestsDataGenerator))]
        public void CheckIfIsNotEmptyMethodReturnValidValuesWhenEnumerableIsEmpty(
            IEnumerable<object> emptyOrNullEnumerable)
        {
            var testResult = emptyOrNullEnumerable.IsNotEmpty();
            testResult.Should().BeFalse();
        }

        [Fact]
        public void CheckIfIsEmptyOrIsNotEmptyMethodReturnValidValuesWhenListIsNotEmpty()
        {
            var notEmptyList = new List<object> {new object()};

            var testResult = notEmptyList.IsEmpty();
            testResult.Should().BeFalse();

            testResult = notEmptyList.IsNotEmpty();
            testResult.Should().BeTrue();
        }

        [Fact]
        public void CheckIfIsEmptyOrIsNotEmptyMethodReturnValidValuesWhenEnumerableIsNotEmpty()
        {
            IEnumerable<object> notEmptyEnumerable = new List<object> {new object()};

            var testResult = notEmptyEnumerable.IsEmpty();
            testResult.Should().BeFalse();

            testResult = notEmptyEnumerable.IsNotEmpty();
            testResult.Should().BeTrue();
        }

        private class CollectionExtensionsTestsDataGenerator
        {
            public static List<object[]> EmptyLists =>
                new List<object[]>
                {
                    new object[] {new List<object>()},
                    new object[] {null}
                };

            public static IEnumerable<object[]> EmptyEnumerable =>
                new List<object[]>
                {
                    new object[] {new List<object>()},
                    new object[] {null}
                };
        }
    }
}