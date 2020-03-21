using FluentAssertions;
using Shoppingendly.Services.Products.Infrastructure.Extensions;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Extensions
{
    public class GetGenericNameExtensions
    {
        [Fact]
        public void CheckIfGetGenericNameCouldReturnAValidValue()
        {
            // Arrange
            var exampleObject = new object();
            
            // Act
            var testResult = exampleObject.GetGenericTypeName();

            // Assert
            testResult.Should().Be("Object");
        }
    }
}