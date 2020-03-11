using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Application.Mapper.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ConstructUsing(r => new RoleDto(r.Id.ToString(), r.Name));
        }
    }
}