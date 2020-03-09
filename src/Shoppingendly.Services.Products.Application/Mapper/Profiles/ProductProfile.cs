using System.Linq;
using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Application.Mapper.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ConstructUsing((p, context)
                    => new ProductDto
                    {
                        Id = p.Id.Id.ToString(),
                        Name = p.Name,
                        Producer = p.Producer,
                        Icon = context.Mapper
                            .Map<Picture, PictureDto>(p.Picture)
                    });

            CreateMap<Product, ProductDetailsDto>()
                .ConstructUsing((p, context)
                    => new ProductDetailsDto
                    {
                        Id = p.Id.Id.ToString(),
                        Name = p.Name,
                        Producer = p.Producer,
                        Picture = context.Mapper
                            .Map<Picture, PictureDto>(p.Picture),
                        Categories = p.ProductCategories
                            .Select(pc => pc.Category.Name)
                    });
        }
    }
}