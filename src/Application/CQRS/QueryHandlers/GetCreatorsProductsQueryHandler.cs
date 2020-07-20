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

using System.Threading.Tasks;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.Application.DTO;
using SoftSentre.Shoppingendly.Services.Products.Application.Mapper;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Results;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Application.CQRS.QueryHandlers
{
    public class GetCreatorsProductsQueryHandler : IQueryHandler<GetCreatorsProductsQuery, CreatorWithProductsDto>
    {
        private readonly ICreatorDomainController _creatorDomainController;
        private readonly IMapperWrapper _mapperWrapper;

        public GetCreatorsProductsQueryHandler(ICreatorDomainController creatorDomainController,
            IMapperWrapper mapperWrapper)
        {
            _creatorDomainController = creatorDomainController;
            _mapperWrapper = mapperWrapper.IfEmptyThenThrowOrReturnValue();
        }

        public async Task<IQueryResult<CreatorWithProductsDto>> QueryAsync(GetCreatorsProductsQuery query)
        {
            var creatorWithProducts = await _creatorDomainController.GetCreatorWithProductsByIdAsync(query.CreatorId);

            if (creatorWithProducts.HasNoValue)
            {
                return QueryResult<CreatorWithProductsDto>.Failed("Creator doesn't exists.");
            }

            var creatorWithProductsDto = _mapperWrapper.MapCreatorToCreatorWithProductsDto(creatorWithProducts.Value);

            return creatorWithProductsDto == null
                ? QueryResult<CreatorWithProductsDto>.Failed("Mapping from creator to creatorWithProductsDto failed.")
                : QueryResult<CreatorWithProductsDto>.Success(creatorWithProductsDto);
        }
    }
}