using System.Linq;
using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Entities;

namespace Shoppingendly.Services.Products.Application.Mapper.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, BasicCategoryDto>()
                .ConstructUsing(c => new BasicCategoryDto
                {
                    Id = c.Id.Id.ToString(),
                    Name = c.Name
                });

            CreateMap<Category, CategoryDto>()
                .ConstructUsing(c => new CategoryDto
                {
                    Id = c.Id.Id.ToString(),
                    Name = c.Name,
                    Description = c.Description
                });

            CreateMap<Category, CategoryWithProductsDto>()
                .ConstructUsing(c => new CategoryWithProductsDto
                {
                    Id = c.Id.Id.ToString(),
                    Name = c.Name,
                    Description = c.Description,
                    Products = c.ProductCategories.Select(pc => pc.Product)
                });
        }
    }
}