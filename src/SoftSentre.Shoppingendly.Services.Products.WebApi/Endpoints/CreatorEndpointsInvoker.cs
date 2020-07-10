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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.Application.DTO;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Bus;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.WebApi.Endpoints
{
    public class CreatorEndpointsInvoker
    {
        private readonly IQueryBus _queryBus;

        public CreatorEndpointsInvoker(IQueryBus queryBus)
        {
            _queryBus = queryBus.IfEmptyThenThrowAndReturnValue();
        }

        public async Task GetCreatorProducts(HttpContext context)
        {
            var param = context.Request.RouteValues["creatorId"].ToString();
                                
            if (param.IsEmpty())
                return;
                                
            var creatorGuid = Guid.Parse(param);
            var creatorId = new CreatorId(creatorGuid);
            var query = new GetCreatorsProductsQuery(creatorId);
            var result = await _queryBus.QueryAsync<GetCreatorsProductsQuery, CreatorWithProductsDto>(query);
            
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result.Data,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
        }
    }
}