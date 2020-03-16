namespace Shoppingendly.Services.Products.Application.DTO
{
    public class RoleDto
    {
        public string Id { get; }
        public string Role { get; }

        public RoleDto(string id, string role)
        {
            Id = id;
            Role = role;
        }
    }
}