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
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.AutoMapper.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ConstructUsing((p, context)
                    => new ProductDto(p.Id.Id.ToString(), context.Mapper
                        .Map<Picture, PictureDto>(p.Picture), p.Name, p.Producer.Name));

            CreateMap<Product, ProductDetailsDto>()
                .ConstructUsing((p, context)
                    => new ProductDetailsDto(p.Id.Id.ToString(), p.Name, p.Producer.Name, context.Mapper
                        .Map<Picture, PictureDto>(p.Picture), p.ProductCategories.Select(pc => pc.Category.Name)));
        }
    }
}