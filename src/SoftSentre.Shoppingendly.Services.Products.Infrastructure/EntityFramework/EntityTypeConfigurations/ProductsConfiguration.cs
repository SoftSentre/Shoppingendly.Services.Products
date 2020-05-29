// Copyright 2020 SoftSentre Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Aggregates;
using static SoftSentre.Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
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
                .IsRequired();

            productsConfiguration.Property(p => p.Producer)
                .HasColumnName("ProductProducer")
                .HasMaxLength(ProductProducerMaxLength)
                .IsRequired();

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