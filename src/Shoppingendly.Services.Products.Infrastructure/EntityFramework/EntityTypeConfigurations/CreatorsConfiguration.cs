using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using static Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class CreatorsConfiguration : IEntityTypeConfiguration<Creator>
    {
        public void Configure(EntityTypeBuilder<Creator> creatorsConfiguration)
        {
            creatorsConfiguration.ToTable("Creators", ProductServiceDbContext.DefaultSchema);
            
            creatorsConfiguration.HasKey(c => c.Id);

            creatorsConfiguration.Property(c => c.Name)
                .HasColumnName("CreatorName")
                .HasMaxLength(CreatorNameMaxLength)
                .IsRequired(IsCreatorNameRequired);
            
            creatorsConfiguration.Property(c => c.Email)
                .HasColumnName("CreatorEmail")
                .HasMaxLength(CreatorEmailMaxLength)
                .IsRequired();

            creatorsConfiguration.Property(p => p.RoleId)
                .HasColumnName("CreatorRoleId")
                .IsRequired(IsCreatorEmailRequired);

            creatorsConfiguration.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate");

            creatorsConfiguration.Property(c => c.CreatedAt)
                .HasColumnName("CreatedDate")
                .IsRequired();

            creatorsConfiguration.HasOne(c => c.Role)
                .WithMany()
                .HasForeignKey(c => c.RoleId);
            
            creatorsConfiguration.HasMany(c => c.Products)
                .WithOne(p => p.Creator)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            creatorsConfiguration.Metadata.FindNavigation(nameof(Creator.Products))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            creatorsConfiguration.Ignore(c => c.DomainEvents);
        }
    }
}