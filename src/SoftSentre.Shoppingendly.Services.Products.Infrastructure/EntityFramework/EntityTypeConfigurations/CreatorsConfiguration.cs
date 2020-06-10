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
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations
{
    public class CreatorsConfiguration : IEntityTypeConfiguration<Creator>
    {
        public void Configure(EntityTypeBuilder<Creator> creatorsConfiguration)
        {
            creatorsConfiguration.ToTable("Creators", ProductServiceDbContext.DefaultSchema);

            creatorsConfiguration.HasKey(c => c.CreatorId);

            creatorsConfiguration.Property(c => c.CreatorName)
                .HasColumnName("CreatorName")
                .HasMaxLength(CreatorNameMaxLength)
                .IsRequired();

            creatorsConfiguration.Property(p => p.CreatorRoleId)
                .HasColumnName("CreatorRoleId")
                .IsRequired();

            creatorsConfiguration.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate");

            creatorsConfiguration.Property(c => c.CreatedAt)
                .HasColumnName("CreatedDate")
                .IsRequired();

            creatorsConfiguration.HasOne(c => c.CreatorRole)
                .WithMany()
                .HasForeignKey(c => c.CreatorRoleId);

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