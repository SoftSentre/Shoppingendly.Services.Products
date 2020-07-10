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
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.BasicTypes.Types
{
    public class MaybeTests
    {
        [Fact]
        public void ComparingNullObjectsByEqualsReturnExpectedResult()
        {
            // Arrange
            var firstObject = Maybe<string>.Empty;
            var secondObject = Maybe<string>.Empty;
            var secondObjectNotEmpty = new Maybe<string>("Value");

            // Act & Assert
            var equals = firstObject.Equals(secondObject);
            equals.Should().BeTrue();
            equals = firstObject.Equals(secondObjectNotEmpty);
            equals.Should().BeFalse();
        }

        [Fact]
        public void InvalidOperationExceptionShouldBeThrownWhenTryingToGetValueFromNullObject()
        {
            // Arrange
            var maybe = Maybe<object>.Empty;
            Maybe<object> secondMaybe = null;

            // Act
            Action action = () => maybe = secondMaybe.Value;

            // Assert
            maybe.HasNoValue.Should().BeTrue();
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ComparingObjectsByHashCodesReturnExpectedResult()
        {
            // Arrange
            var firstObject = new Maybe<string>("Value");
            var secondObject = new Maybe<string>("Value");
            const string secondObjectNotWrapped = "Value";
            var secondOtherObject = new Maybe<string>("Value1");
            const string secondOtherObjectNotWrapped = "Value1";

            // Act & Assert
            var equals = firstObject.GetHashCode() == secondObject.GetHashCode();
            equals.Should().BeTrue();

            equals = firstObject.GetHashCode() == secondObjectNotWrapped.GetHashCode();
            equals.Should().BeTrue();

            equals = firstObject.GetHashCode() == secondOtherObject.GetHashCode();
            equals.Should().BeFalse();

            equals = firstObject.GetHashCode() == secondOtherObjectNotWrapped.GetHashCode();
            equals.Should().BeFalse();
        }

        [Fact]
        public void ComparingObjectsByEqualsReturnExpectedResult()
        {
            // Arrange
            var firstObject = new Maybe<string>("Value");
            var secondObject = new Maybe<string>("Value");
            const string secondObjectNotWrapped = "Value";
            var secondOtherObject = new Maybe<string>("Value1");
            const string secondOtherObjectNotWrapped = "Value1";

            // Act & Assert
            var equals = firstObject.Equals(secondObject);
            equals.Should().BeTrue();

            equals = firstObject.Equals(secondObjectNotWrapped);
            equals.Should().BeTrue();

            equals = firstObject.Equals(secondOtherObject);
            equals.Should().BeFalse();

            equals = firstObject.Equals(secondOtherObjectNotWrapped);
            equals.Should().BeFalse();

            equals = firstObject.Equals(new object());
            equals.Should().BeFalse();
        }

        [Fact]
        public void ComparingObjectsByReferenceEqualOperatorsReturnExpectedResult()
        {
            // Arrange
            var firstObject = new Maybe<string>("Value");
            var secondObject = new Maybe<string>("Value");
            const string secondObjectNotWrapped = "Value";
            var secondOtherObject = new Maybe<string>("Value1");
            const string secondOtherObjectNotWrapped = "Value1";

            // Act & Assert
            var equals = firstObject == secondObject;
            equals.Should().BeTrue();

            equals = firstObject == secondObjectNotWrapped;
            equals.Should().BeTrue();

            equals = firstObject == secondOtherObject;
            equals.Should().BeFalse();

            equals = firstObject == secondOtherObjectNotWrapped;
            equals.Should().BeFalse();
        }

        [Fact]
        public void ComparingObjectsByReferenceNotEqualOperatorsReturnExpectedResult()
        {
            // Arrange
            var firstObject = new Maybe<string>("Value");
            var secondObject = new Maybe<string>("Value");
            const string secondObjectNotWrapped = "Value";
            var secondOtherObject = new Maybe<string>("Value1");
            const string secondOtherObjectNotWrapped = "Value1";

            // Act & Assert
            var equals = firstObject != secondObject;
            equals.Should().BeFalse();

            equals = firstObject != secondObjectNotWrapped;
            equals.Should().BeFalse();

            equals = firstObject != secondOtherObject;
            equals.Should().BeTrue();

            equals = firstObject != secondOtherObjectNotWrapped;
            equals.Should().BeTrue();
        }

        [Fact]
        public void ToStringMethodShouldReturnDefaultValue()
        {
            // Arrange
            var maybe = new Maybe<object>();

            // Act
            var testResult = maybe.ToString();

            // Assert
            testResult.Should().Be("No value");
        }
    }
}