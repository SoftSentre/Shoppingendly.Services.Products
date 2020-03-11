namespace Shoppingendly.Services.Products.Application.DTO
{
    public class BasicCategoryDto
    {
        public string Id { get; }
        public string Name { get; }

        public BasicCategoryDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}