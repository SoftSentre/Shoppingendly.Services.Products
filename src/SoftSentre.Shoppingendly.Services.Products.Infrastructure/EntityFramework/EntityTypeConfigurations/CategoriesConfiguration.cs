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
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Entities;
using static SoftSentre.Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoriesConfiguration)
        {
            categoriesConfiguration.ToTable("Categories", ProductServiceDbContext.DefaultSchema);

            categoriesConfiguration.HasKey(c => c.Id);

            categoriesConfiguration.Property(c => c.Name)
                .HasColumnName("CategoryName")
                .HasMaxLength(CategoryNameMaxLength)
                .IsRequired();

            categoriesConfiguration.Property(c => c.Description)
                .HasColumnName("CategoryDescription")
                .HasMaxLength(CategoryDescriptionMaxLength)
                .IsRequired(IsCategoryDescriptionRequired);

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