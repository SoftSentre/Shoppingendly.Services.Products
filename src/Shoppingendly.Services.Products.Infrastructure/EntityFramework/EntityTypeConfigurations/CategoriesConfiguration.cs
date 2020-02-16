using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Entities;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoriesConfiguration)
        {
            categoriesConfiguration.ToTable("Categories", ProductServiceDbContext.DefaultSchema);

            categoriesConfiguration.HasKey(c => c.Id);

            categoriesConfiguration.Property(c => c.Name)
                .HasColumnName("CategoryName")
                .HasMaxLength(30)
                .IsRequired();

            categoriesConfiguration.Property(c => c.Description)
                .HasColumnName("CategoryDescription")
                .HasMaxLength(4000);
            
            categoriesConfiguration.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate");

            categoriesConfiguration.Property(c => c.CreatedAt)
                .HasColumnName("CreatedDate")
                .IsRequired();

            categoriesConfiguration.HasMany(c => c.ProductCategories)
                .WithOne(pc => pc.Category)
                .HasForeignKey(pc => pc.SecondKey);
            
            categoriesConfiguration.Metadata.FindNavigation(nameof(Category.ProductCategories))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            categoriesConfiguration.Ignore(c => c.DomainEvents); 
        }
    }
}