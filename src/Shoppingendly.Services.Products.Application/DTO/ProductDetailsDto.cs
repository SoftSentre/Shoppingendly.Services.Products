using System.Collections.Generic;

namespace Shoppingendly.Services.Products.Application.DTO
{
    public class ProductDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public PictureDto Picture{ get; set; }
    }
}