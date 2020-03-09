using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

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
                .ConstructUsing((c, context)
                    => new CreatorDto
                    {
                        Id = c.Id.Id.ToString(),
                        Name = c.Name,
                        Role = context.Mapper
                            .Map<Role, RoleDto>(c.Role)
                    });

            CreateMap<Creator, CreatorWithProducts>()
                .ConstructUsing((c, context)
                    => new CreatorWithProducts
                    {
                        Id = c.Id.Id.ToString(),
                        Name = c.Name,
                        Role = context.Mapper
                            .Map<Role, RoleDto>(c.Role),
                        Products = c.Products
                    });
        }
    }
}