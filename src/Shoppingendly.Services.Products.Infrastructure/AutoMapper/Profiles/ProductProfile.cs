using System.Linq;
using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Infrastructure.AutoMapper.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ConstructUsing((p, context)
                    => new ProductDto(p.Id.Id.ToString(), context.Mapper
                        .Map<Picture, PictureDto>(p.Picture), p.Name, p.Producer));

            CreateMap<Product, ProductDetailsDto>()
                .ConstructUsing((p, context)
                    => new ProductDetailsDto(p.Id.Id.ToString(), p.Name, p.Producer, context.Mapper
                        .Map<Picture, PictureDto>(p.Picture), p.ProductCategories.Select(pc => pc.Category.Name)));
        }
    }
}