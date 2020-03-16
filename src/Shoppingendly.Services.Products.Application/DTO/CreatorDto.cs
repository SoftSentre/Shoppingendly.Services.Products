namespace Shoppingendly.Services.Products.Application.DTO
{
    public class CreatorDto : BasicCreatorDto
    {
        public RoleDto Role { get; }

        public CreatorDto(string id, string name, RoleDto role) : base(id, name)
        {
            Role = role;
        }
    }
}