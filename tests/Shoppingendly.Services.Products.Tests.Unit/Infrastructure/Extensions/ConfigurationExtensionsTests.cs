using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Shoppingendly.Services.Products.Infrastructure.Extensions;
using Shoppingendly.Services.Products.Infrastructure.Options;
using Xunit;

namespace Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Extensions
{
    public class ConfigurationExtensionsTest
    {
        private readonly IConfiguration _configuration =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();


        [Fact]
        public void CheckIfGetOptionsMethodReturningCorrectValuesFromConfiguration()
        {
            // Arrange

            // Act
            var appOptions = _configuration.GetOptions<AppOptions>("app");

            // Assert
            appOptions.Name.Should().Be("Shoppingendly Products Service");
            appOptions.Service.Should().Be("products-service");
            appOptions.Version.Should().Be("1");
        }
    }
}