using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Entities;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductCategoriesConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> productCategoriesConfiguration)
        {
            productCategoriesConfiguration.ToTable("ProductCategories", ProductServiceDbContext.DefaultSchema);

            productCategoriesConfiguration.HasKey(pc => new {pc.FirstKey, pc.SecondKey});

            productCategoriesConfiguration.Property(pc => pc.UpdatedDate)
                .HasColumnName("UpdatedDate");

            productCategoriesConfiguration.Property(pc => pc.CreatedAt)
                .HasColumnName("CreatedDate")
                .IsRequired();
        }
    }
}