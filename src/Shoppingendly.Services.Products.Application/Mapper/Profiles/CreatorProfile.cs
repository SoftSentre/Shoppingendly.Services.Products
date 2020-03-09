using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Entities;

namespace Shoppingendly.Services.Products.Application.Mapper.Profiles
{
    public class CreatorProfile : Profile
    {
        public CreatorProfile()
        {
            CreateMap<Creator, BasicCreatorDto>()
                .ConstructUsing(c => new BasicCreatorDto
                {
                    Id = c.Id.Id.ToString(),
                    Name = c.Name
                });

            CreateMap<Creator, CreatorDto>()
                .ConstructUsing(c => new CreatorDto
                {
                    Id = c.Id.Id.ToString(),
                    Name = c.Name,
                    Role = c.Role.Name
                });

            CreateMap<Creator, CreatorWithProducts>()
                .ConstructUsing(c => new CreatorWithProducts
                {
                    Id = c.Id.Id.ToString(),
                    Name = c.Name,
                    Role = c.Role.Name,
                    Products = c.Products
                });
        }
    }
}