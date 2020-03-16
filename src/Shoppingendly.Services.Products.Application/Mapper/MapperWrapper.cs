using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Application.Mapper.Base;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Application.Mapper
{
    public class MapperWrapper : IMapperWrapper
    {
        private readonly IMapper _mapper;

        public MapperWrapper(IMapper mapper)
        {
            _mapper = mapper.IfEmptyThenThrowAndReturnValue();
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

        public RoleDto MapRoleToRoleDto(Role role)
        {
            return _mapper.Map<Role, RoleDto>(role);
        }

        public PictureDto MapPictureToPictureDto(Picture picture)
        {
            return _mapper.Map<Picture, PictureDto>(picture);
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