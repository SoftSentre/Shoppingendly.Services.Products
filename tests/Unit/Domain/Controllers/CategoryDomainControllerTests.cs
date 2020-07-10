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
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Domain.Controllers
{
    public class CategoryDomainControllerTests : IAsyncLifetime
    {
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<ICategoryBusinessRulesChecker> _categoryBusinessRulesCheckerMock;
        private Mock<IDomainEventEmitter> _domainEventEmitterMock;
        private CategoryFactory _categoryFactory;
        private Category _category;
        private CategoryId _categoryId;
        //private CategoryId _parentCategoryId;
        private string _categoryName;
        //private string _newCategoryName;
        private string _categoryDescription;
        //private string _newCategoryDescription;
        private Picture _categoryIcon;
        
        public async Task InitializeAsync()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryBusinessRulesCheckerMock = new Mock<ICategoryBusinessRulesChecker>();
            _domainEventEmitterMock = new Mock<IDomainEventEmitter>();
            _categoryFactory =
                new CategoryFactory(_categoryBusinessRulesCheckerMock.Object, _domainEventEmitterMock.Object);
            _categoryId = new CategoryId(new Guid("F3CCBF64-8398-4359-80EF-64DCDF55D15B"));
            _categoryName = "exampleCategoryName";
            //_newCategoryName = "newExampleCategoryName";
            _categoryDescription = "exampleCategoryDescription";
            //_newCategoryDescription = "newExampleCategoryDescription";
            _categoryIcon = Picture.Create("exampleCategoryIconName", "exampleCategoryIconUrl");
            _category = _categoryFactory.Create(_categoryId, _categoryName, _categoryDescription, _categoryIcon);
        
            await Task.CompletedTask;
        }

        [Fact]
        public async Task GetCategoryShouldReturnCorrectResult()
        {
            // Arrange
            _categoryRepositoryMock.Setup(cr => cr.GetByIdAsync(_categoryId))
                .ReturnsAsync(_category);

            ICategoryDomainController categoryDomainController =
                new CategoryDomainController(_categoryRepositoryMock.Object, _categoryBusinessRulesCheckerMock.Object,
                    _categoryFactory, _domainEventEmitterMock.Object);

            // Act
            var category = await categoryDomainController.GetCategoryByIdAsync(_categoryId);

            // Assert
            category.Should().Be(_category);
            _categoryRepositoryMock.Verify(cr => cr.GetByIdAsync(_categoryId), Times.Once);
        }
        
        public async Task DisposeAsync()
        {
            _categoryRepositoryMock = null;
            _categoryBusinessRulesCheckerMock = null;
            _domainEventEmitterMock = null;
            _categoryFactory = null;
            _categoryId = null;
            _categoryName = null;
            //_newCategoryName = null;
            _categoryDescription = null;
            //_newCategoryDescription = null;
            _categoryIcon = null;
            _category = null;
        
            await Task.CompletedTask;
        }
    }
}