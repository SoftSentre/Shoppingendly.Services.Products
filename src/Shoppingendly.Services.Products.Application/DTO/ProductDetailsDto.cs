using System.Collections.Generic;

namespace Shoppingendly.Services.Products.Application.DTO
{
    public class ProductDetailsDto
    {
        public string Id { get; }
        public string Name { get; }
        public string Producer { get; }
        public PictureDto Picture { get; }
        public IEnumerable<string> Categories { get; }

        public ProductDetailsDto(string id, string name, string producer, PictureDto picture,
            IEnumerable<string> categories)
        {
            Id = id;
            Name = name;
            Producer = producer;
            Picture = picture;
            Categories = categories;
        }
    }
}