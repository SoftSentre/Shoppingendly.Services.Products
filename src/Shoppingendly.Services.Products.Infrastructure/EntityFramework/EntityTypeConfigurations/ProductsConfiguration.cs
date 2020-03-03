using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using static Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> productsConfiguration)
        {
            productsConfiguration.ToTable("Products", ProductServiceDbContext.DefaultSchema);

            productsConfiguration.HasKey(p => p.Id);

            productsConfiguration.Property(p => p.Name)
                .HasColumnName("ProductName")
                .HasMaxLength(ProductNameMaxLength)
                .IsRequired(IsProductNameRequired);

            productsConfiguration.Property(p => p.Producer)
                .HasColumnName("ProductProducer")
                .HasMaxLength(ProductProducerMaxLength)
                .IsRequired(IsProductProducerRequired);

            productsConfiguration.Property(p => p.CreatorId)
                .HasColumnName("ProductCreatorId")
                .IsRequired();

            productsConfiguration.Property(p => p.UpdatedDate)
                .HasColumnName("UpdatedDate");

            productsConfiguration.Property(p => p.CreatedAt)
                .HasColumnName("CreatedDate")
                .IsRequired();

            productsConfiguration.OwnsOne(
                p => p.Picture, 
                pi =>
                {
                    pi.Property(pp => pp.Name)
                        .HasColumnName("ProductPictureName")
                        .HasMaxLength(PictureNameMaxLength);

                    pi.Property(pp => pp.Url)
                        .HasColumnName("ProductPictureUrl")
                        .HasMaxLength(PictureUrlMaxLength);

                    pi.Property(pp => pp.IsEmpty)
                        .HasColumnName("IsProductPictureEmpty");
                });

            productsConfiguration.HasMany(p => p.ProductCategories)
                .WithOne(pc => pc.Product)
                .HasForeignKey(pc => pc.FirstKey);
            
            productsConfiguration.Metadata.FindNavigation(nameof(Product.ProductCategories))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            
            productsConfiguration.Ignore(c => c.DomainEvents);
        }
    }
}