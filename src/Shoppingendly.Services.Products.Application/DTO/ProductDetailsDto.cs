namespace Shoppingendly.Services.Products.Application.DTO
{
    public class ProductDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public string Category { get; set; }
        public PictureDto Picture{ get; set; }
    }
}