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
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoriesConfiguration)
        {
            categoriesConfiguration.ToTable("Categories", ProductServiceDbContext.DefaultSchema);

            categoriesConfiguration.HasKey(c => c.CategoryId);

            categoriesConfiguration.Property(c => c.ParentCategoryId)
                .HasColumnName("ParentCategoryId")
                .IsRequired(false);

            categoriesConfiguration.Property(c => c.CategoryName)
                .HasColumnName("CategoryName")
                .HasMaxLength(CategoryNameMaxLength)
                .IsRequired();

            categoriesConfiguration.Property(c => c.CategoryDescription)
                .HasColumnName("CategoryDescription")
                .HasMaxLength(CategoryDescriptionMaxLength)
                .IsRequired(IsCategoryDescriptionRequired);

            categoriesConfiguration.OwnsOne(
                p => p.CategoryIcon,
                pi =>
                {
                    pi.Property(pp => pp.Name)
                        .HasColumnName("CategoryIconName")
                        .HasMaxLength(PictureNameMaxLength);

                    pi.Property(pp => pp.Url)
                        .HasColumnName("CategoryIconUrl")
                        .HasMaxLength(PictureUrlMaxLength);

                    pi.Property(pp => pp.IsEmpty)
                        .HasColumnName("IsCategoryIconEmpty");
                });

            categoriesConfiguration.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate");

            categoriesConfiguration.Property(c => c.CreatedAt)
                .HasColumnName("CreatedDate")
                .IsRequired();

            categoriesConfiguration.HasMany(c => c.AssignedProducts)
                .WithOne(pc => pc.Category)
                .HasForeignKey(pc => pc.CategoryId);

            categoriesConfiguration.Metadata.FindNavigation(nameof(Category.AssignedProducts))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            categoriesConfiguration.Ignore(c => c.DomainEvents);
        }
    }
}