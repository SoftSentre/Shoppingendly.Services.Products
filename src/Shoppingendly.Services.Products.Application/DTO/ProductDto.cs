namespace Shoppingendly.Services.Products.Application.DTO
{
    public class ProductDto
    {
        public string Id { get; }
        public PictureDto Icon { get; }
        public string Name { get; }
        public string Producer { get; }

        public ProductDto(string id, PictureDto icon, string name, string producer)
        {
            Id = id;
            Icon = icon;
            Name = name;
            Producer = producer;
        }
    }
}