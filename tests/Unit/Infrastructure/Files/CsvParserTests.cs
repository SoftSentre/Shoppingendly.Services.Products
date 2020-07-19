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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.Files.Csv;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Files
{
    public class CsvParserTests : IAsyncLifetime
    {
        private ICsvParser _csvParser;

        public async Task InitializeAsync()
        {
            var contentRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            _csvParser = new CsvParser(contentRoot);

            await Task.CompletedTask;
        }

        [Fact]
        public void CheckIfLoadCreatorRoleMethodsReturnValidObject()
        {
            // Arrange
            var roles = new List<CreatorRole>
            {
                CreatorRole.Admin,
                CreatorRole.Moderator,
                CreatorRole.User
            };

            // Act
            var result = _csvParser.LoadCreatorRoles();
            
            // Assert
            result.Should().BeEquivalentTo(roles);
        }
        
        public async Task DisposeAsync()
        {
            _csvParser = null;

            await Task.CompletedTask;
        }
    }
}