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

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.AutoMapper.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, BasicCategoryDto>()
                .ConstructUsing(c =>
                    new BasicCategoryDto(c.CategoryId.Id.ToString(), c.CategoryName));

            CreateMap<Category, CategoryDto>()
                .ConstructUsing(c => new CategoryDto(c.CategoryId.Id.ToString(), c.CategoryName, c.CategoryDescription));

            CreateMap<Category, CategoryWithProductsDto>()
                .ConstructUsing((c, context) => new CategoryWithProductsDto(c.CategoryId.Id.ToString(), c.CategoryName,
                    c.CategoryDescription,
                    c.ProductCategories.Select(pc => context.Mapper.Map<Product, ProductDto>(pc.Product))));
        }
    }
}