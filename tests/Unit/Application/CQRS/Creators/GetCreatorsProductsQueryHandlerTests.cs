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
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.QueryHandlers;
using SoftSentre.Shoppingendly.Services.Products.Application.DTO;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Exceptions;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Application.CQRS.Base;
using SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Infrastructure.Logger.Helpers;
using Xunit;

namespace SoftSentre.Shoppingendly.Services.Products.Tests.Unit.Application.CQRS.Creators
{
    public class
        GetCreatorsProductsQueryHandlerTests : QueryHandlerTestsStartUp<GetCreatorsProductsQuery, CreatorWithProductsDto
        >
    {
        private GetCreatorsProductsQuery _getCreatorsProductsQuery;
        private CreatorWithProductsDto _creatorWithProductsDto;
        private Creator _creator;
        private Mock<ICreatorDomainController> _creatorDomainControllerMock;
        private IQueryHandler<GetCreatorsProductsQuery, CreatorWithProductsDto> _queryHandler;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var creatorId = new CreatorId(new Guid("6CA84FFA-903A-48E3-94F4-9CF27E6A4DB2"));
            var creatorRole = CreatorRole.User;
            const string creatorName = "Creator";

            _getCreatorsProductsQuery = new GetCreatorsProductsQuery(creatorId);
            _creator = new Creator(creatorId, creatorName, creatorRole);

            _creatorWithProductsDto = new CreatorWithProductsDto(
                new Guid("6CA84FFA-903A-48E3-94F4-9CF27E6A4DB2").ToString("N"), creatorName,
                new RoleDto(creatorRole.Id.ToString(), creatorRole.Name), new List<ProductDto>
                {
                    new ProductDto(new Guid("6BCC839D-95AC-4325-8B04-0FB5B8579B36").ToString("N"),
                        new PictureDto(string.Empty), "product", "company"),
                    new ProductDto(new Guid("E844A9F5-22E8-4D73-98F1-96BD3CEDA6A3").ToString("N"),
                        new PictureDto(string.Empty), "someStuff", "company"),
                    new ProductDto(new Guid("009053DE-5B9B-4E51-BD6B-E754436F162B").ToString("N"),
                        new PictureDto(string.Empty), "niceItem", "firm")
                }.Paginate());

            _creatorDomainControllerMock = new Mock<ICreatorDomainController>();

            await Task.CompletedTask;
        }

        public override async Task DisposeAsync()
        {
            await base.DisposeAsync();
            _creatorDomainControllerMock = null;

            await Task.CompletedTask;
        }

        [Fact]
        public async Task HandleAsyncShouldFailedWhenCustomExceptionThrown()
        {
            // Arrange
            _creatorDomainControllerMock
                .Setup(cdc => cdc.GetCreatorWithProductsByIdAsync(_getCreatorsProductsQuery.CreatorId))
                .Throws<DomainException>();

            _queryHandler = new LoggingQueryHandlerDecorator<GetCreatorsProductsQuery, CreatorWithProductsDto>(
                new GetCreatorsProductsQueryHandler(_creatorDomainControllerMock.Object, MapperWrapperMock.Object),
                LoggerMock.Object);

            // Act
            var queryResult = await _queryHandler.QueryAsync(_getCreatorsProductsQuery);

            // Assert
            queryResult.Ok.Should().BeFalse();
            queryResult.Message.Should().NotBeEmpty();
            queryResult.Data.Should().Be(null);

            LoggerMock.Verify(LogLevel.Information, Times.Once());
            LoggerMock.Verify(LogLevel.Error, Times.Once());
            _creatorDomainControllerMock.Verify(cdc =>
                cdc.GetCreatorWithProductsByIdAsync(_getCreatorsProductsQuery.CreatorId), Times.Once);
            MapperWrapperMock.Verify(mw => mw.MapCreatorToCreatorWithProductsDto(It.IsAny<Creator>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsyncShouldFailedWhenExceptionThrown()
        {
            // Arrange
            _creatorDomainControllerMock
                .Setup(cdc => cdc.GetCreatorWithProductsByIdAsync(_getCreatorsProductsQuery.CreatorId))
                .Throws<Exception>();

            _queryHandler = new LoggingQueryHandlerDecorator<GetCreatorsProductsQuery, CreatorWithProductsDto>(
                new GetCreatorsProductsQueryHandler(_creatorDomainControllerMock.Object, MapperWrapperMock.Object),
                LoggerMock.Object);

            // Act
            var queryResult = await _queryHandler.QueryAsync(_getCreatorsProductsQuery);

            // Assert
            queryResult.Ok.Should().BeFalse();
            queryResult.Message.Should().NotBeEmpty();
            queryResult.Data.Should().Be(null);

            LoggerMock.Verify(LogLevel.Information, Times.Once());
            LoggerMock.Verify(LogLevel.Error, Times.Once());
            _creatorDomainControllerMock.Verify(cdc =>
                cdc.GetCreatorWithProductsByIdAsync(_getCreatorsProductsQuery.CreatorId), Times.Once);
            MapperWrapperMock.Verify(mw => mw.MapCreatorToCreatorWithProductsDto(It.IsAny<Creator>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsyncShouldReturnCreatorWithProductsInPositiveScenario()
        {
            // Arrange
            _creatorDomainControllerMock
                .Setup(cdc => cdc.GetCreatorWithProductsByIdAsync(_getCreatorsProductsQuery.CreatorId))
                .ReturnsAsync(_creator);

            MapperWrapperMock.Setup(mm => mm.MapCreatorToCreatorWithProductsDto(It.IsAny<Creator>()))
                .Returns(_creatorWithProductsDto);

            _queryHandler = new LoggingQueryHandlerDecorator<GetCreatorsProductsQuery, CreatorWithProductsDto>(
                new GetCreatorsProductsQueryHandler(_creatorDomainControllerMock.Object, MapperWrapperMock.Object),
                LoggerMock.Object);

            // Act
            var queryResult = await _queryHandler.QueryAsync(_getCreatorsProductsQuery);

            // Assert
            queryResult.Ok.Should().BeTrue();
            queryResult.Errors.Should().BeNull();
            queryResult.Data.Should().Be(_creatorWithProductsDto);

            LoggerMock.Verify(LogLevel.Information, Times.Exactly(2));
            _creatorDomainControllerMock.Verify(cdc =>
                cdc.GetCreatorWithProductsByIdAsync(_getCreatorsProductsQuery.CreatorId), Times.Once);
            MapperWrapperMock.Verify(mw => mw.MapCreatorToCreatorWithProductsDto(It.IsAny<Creator>()), Times.Once);
        }
    }
}