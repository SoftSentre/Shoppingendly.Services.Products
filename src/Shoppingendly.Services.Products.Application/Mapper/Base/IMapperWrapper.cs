using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Application.Mapper.Base
{
    public interface IMapperWrapper
    {
        BasicCategoryDto MapCategoryToBasicCategoryDto(Category category);
        CategoryDto MapCategoryToCategoryDto(Category category);
        CategoryWithProductsDto MapCategoryToCategoryWithProductsDto(Category category);

        BasicCreatorDto MapCreatorToBasicCreatorDto(Creator creator);
        CreatorDto MapCreatorToCreatorDto(Creator creator);
        CreatorWithProducts MapCreatorToCreatorWithProductsDto(Creator creator);

        RoleDto MapRoleToRoleDto(Role role);

        PictureDto MapPictureToPictureDto(Picture picture);

        ProductDto MapProductToProductDto(Product product);
        ProductDetailsDto MapProductToProductDetailsDto(Product product);
    }
}