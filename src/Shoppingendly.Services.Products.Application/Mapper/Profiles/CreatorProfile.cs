using System.Linq;
using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Application.Mapper.Profiles
{
    public class CreatorProfile : Profile
    {
        public CreatorProfile()
        {
            CreateMap<Creator, BasicCreatorDto>()
                .ConstructUsing(c => new BasicCreatorDto(c.Id.Id.ToString(), c.Name));

            CreateMap<Creator, CreatorDto>()
                .ConstructUsing((c, context)
                    => new CreatorDto(c.Id.Id.ToString(), c.Name, context.Mapper.Map<Role, RoleDto>(c.Role)));

            CreateMap<Creator, CreatorWithProductsDto>()
                .ConstructUsing((c, context)
                    => new CreatorWithProductsDto(c.Id.Id.ToString(), c.Name, context.Mapper.Map<Role, RoleDto>(c.Role),
                        c.Products.Select(p => context.Mapper.Map<Product, ProductDto>(p))));
        }
    }
}