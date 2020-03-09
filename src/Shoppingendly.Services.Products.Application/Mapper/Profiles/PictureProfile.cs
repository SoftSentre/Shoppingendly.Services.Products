using AutoMapper;
using Shoppingendly.Services.Products.Application.DTO;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace Shoppingendly.Services.Products.Application.Mapper.Profiles
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<Picture, PictureDto>()
                .ConstructUsing(p => new PictureDto
                {
                    Url = p.Url
                });
        }
    }
}