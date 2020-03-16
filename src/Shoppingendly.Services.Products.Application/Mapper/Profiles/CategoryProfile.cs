using System.Linq;
using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;

namespace Shoppingendly.Services.Products.Application.Mapper.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, BasicCategoryDto>()
                .ConstructUsing(c =>
                    new BasicCategoryDto(c.Id.Id.ToString(), c.Name));

            CreateMap<Category, CategoryDto>()
                .ConstructUsing(c => new CategoryDto(c.Id.Id.ToString(), c.Name, c.Description));

            CreateMap<Category, CategoryWithProductsDto>()
                .ConstructUsing((c, context) => new CategoryWithProductsDto(c.Id.Id.ToString(), c.Name, c.Description,
                    c.ProductCategories.Select(pc => context.Mapper.Map<Product, ProductDto>(pc.Product))));
        }
    }
}