namespace Shoppingendly.Services.Products.Application.DTO
{
    public class CategoryDto : BasicCategoryDto
    {
        public string Description { get; }

        public CategoryDto(string id, string name, string description) : base(id, name)
        {
            Description = description;
        }
    }
}