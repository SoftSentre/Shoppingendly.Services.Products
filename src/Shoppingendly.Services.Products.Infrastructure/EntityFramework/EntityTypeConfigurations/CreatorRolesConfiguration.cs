using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using static Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class CreatorRolesConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> creatorRolesConfiguration)
        {
            creatorRolesConfiguration.ToTable("CreatorRoles", ProductServiceDbContext.DefaultSchema);

            creatorRolesConfiguration.HasKey(cr => cr.Id);

            creatorRolesConfiguration.Property(cr => cr.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            creatorRolesConfiguration.Property(cr => cr.Name)
                .HasMaxLength(RoleNameMaxLength)
                .IsRequired();
        }
    }
}