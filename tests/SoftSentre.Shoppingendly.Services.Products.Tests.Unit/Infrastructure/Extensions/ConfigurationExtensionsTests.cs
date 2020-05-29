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
using Microsoft.Extensions.Configuration;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Options;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Extensions
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