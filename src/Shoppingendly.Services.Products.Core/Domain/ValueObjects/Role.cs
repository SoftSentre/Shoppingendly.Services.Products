using Shoppingendly.Services.Products.Core.Domain.Base.ValueObjects;

namespace Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class Role : Enumeration
    {
        public static readonly Role Admin = new Role(1, "Admin");
        public static readonly Role Moderator = new Role(2, "Moderator");
        public static readonly Role User = new Role(3, "User");

        private Role(int id, string name) : base(id, name)
        {
        }
    }
}