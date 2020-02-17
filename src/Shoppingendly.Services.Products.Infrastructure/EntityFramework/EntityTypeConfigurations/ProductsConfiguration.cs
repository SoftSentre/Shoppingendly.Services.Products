using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> productsConfiguration)
        {
            productsConfiguration.ToTable("Products", ProductServiceDbContext.DefaultSchema);

            productsConfiguration.Property(p => p.Name)
                .HasColumnName("ProductName")
                .HasMaxLength(30)
                .IsRequired();

            productsConfiguration.Property(p => p.Producer)
                .HasColumnName("ProductProducer")
                .HasMaxLength(50)
                .IsRequired();

            productsConfiguration.Property(p => p.CreatorId)
                .HasColumnName("ProductCreatorId")
                .IsRequired();

            productsConfiguration.Property(p => p.UpdatedDate)
                .HasColumnName("UpdatedDate");

            productsConfiguration.Property(p => p.CreatedAt)
                .HasColumnName("CreatedDate")
                .IsRequired();

            productsConfiguration.HasMany(p => p.ProductCategories)
                .WithOne(pc => pc.Product)
                .HasForeignKey(pc => pc.FirstKey);
            
            productsConfiguration.Metadata.FindNavigation(nameof(Product.ProductCategories))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            productsConfiguration.Ignore(c => c.DomainEvents);
        }
    }
}