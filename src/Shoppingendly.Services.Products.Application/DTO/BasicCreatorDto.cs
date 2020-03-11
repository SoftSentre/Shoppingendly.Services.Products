namespace Shoppingendly.Services.Products.Application.DTO
{
    public class BasicCreatorDto
    {
        public string Id { get; }
        public string Name { get; }
        
        public BasicCreatorDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}