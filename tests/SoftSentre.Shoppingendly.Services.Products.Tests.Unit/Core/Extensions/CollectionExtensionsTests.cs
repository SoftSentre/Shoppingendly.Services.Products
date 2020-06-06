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
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Core.Extensions
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

        [Fact]
        public void CheckIfIsEmptyOrIsNotEmptyMethodReturnValidValuesWhenEnumerableIsNotEmpty()
        {
            IEnumerable<object> notEmptyEnumerable = new List<object> {new object()};

            var testResult = notEmptyEnumerable.IsEmpty();
            testResult.Should().BeFalse();

            testResult = notEmptyEnumerable.IsNotEmpty();
            testResult.Should().BeTrue();
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
    }
}