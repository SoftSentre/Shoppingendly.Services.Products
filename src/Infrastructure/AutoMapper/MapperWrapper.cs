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

using AutoMapper;
using SoftSentre.Shoppingendly.Services.Products.Application.DTO;
using SoftSentre.Shoppingendly.Services.Products.Application.Mapper;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.AutoMapper
{
    public class MapperWrapper : IMapperWrapper
    {
        private readonly IMapper _mapper;

        public MapperWrapper(IMapper mapper)
        {
            _mapper = mapper.IfEmptyThenThrowOrReturnValue();
        }

        public BasicCategoryDto MapCategoryToBasicCategoryDto(Category category)
        {
            return _mapper.Map<Category, BasicCategoryDto>(category);
        }

        public CategoryDto MapCategoryToCategoryDto(Category category)
        {
            return _mapper.Map<Category, CategoryDto>(category);
        }

        public CategoryWithProductsDto MapCategoryToCategoryWithProductsDto(Category category)
        {
            return _mapper.Map<Category, CategoryWithProductsDto>(category);
        }

        public BasicCreatorDto MapCreatorToBasicCreatorDto(Creator creator)
        {
            return _mapper.Map<Creator, BasicCreatorDto>(creator);
        }

        public CreatorDto MapCreatorToCreatorDto(Creator creator)
        {
            return _mapper.Map<Creator, CreatorDto>(creator);
        }

        public CreatorWithProductsDto MapCreatorToCreatorWithProductsDto(Creator creator)
        {
            return _mapper.Map<Creator, CreatorWithProductsDto>(creator);
        }

        public RoleDto MapRoleToRoleDto(CreatorRole creatorRole)
        {
            return _mapper.Map<CreatorRole, RoleDto>(creatorRole);
        }

        public PictureDto MapPictureToPictureDto(Picture productPicture)
        {
            return _mapper.Map<Picture, PictureDto>(productPicture);
        }

        public ProductDto MapProductToProductDto(Product product)
        {
            return _mapper.Map<Product, ProductDto>(product);
        }

        public ProductDetailsDto MapProductToProductDetailsDto(Product product)
        {
            return _mapper.Map<Product, ProductDetailsDto>(product);
        }
    }
}