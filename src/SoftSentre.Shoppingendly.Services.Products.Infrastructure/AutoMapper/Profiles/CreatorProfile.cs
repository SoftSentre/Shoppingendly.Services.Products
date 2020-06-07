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

using System.Linq;
using AutoMapper;
using SoftSentre.Shoppingendly.Services.Products.Application.DTO;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.AutoMapper.Profiles
{
    public class CreatorProfile : Profile
    {
        public CreatorProfile()
        {
            CreateMap<Creator, BasicCreatorDto>()
                .ConstructUsing(c => new BasicCreatorDto(c.Id.Id.ToString(), c.CreatorName));

            CreateMap<Creator, CreatorDto>()
                .ConstructUsing((c, context)
                    => new CreatorDto(c.Id.Id.ToString(), c.CreatorName,
                        context.Mapper.Map<CreatorRole, RoleDto>(c.CreatorRole)));

            CreateMap<Creator, CreatorWithProductsDto>()
                .ConstructUsing((c, context)
                    => new CreatorWithProductsDto(c.Id.Id.ToString(), c.CreatorName,
                        context.Mapper.Map<CreatorRole, RoleDto>(c.CreatorRole),
                        c.Products.Select(p => context.Mapper.Map<Product, ProductDto>(p))));
        }
    }
}