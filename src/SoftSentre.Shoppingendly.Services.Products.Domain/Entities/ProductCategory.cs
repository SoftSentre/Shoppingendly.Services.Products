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

using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Entities
{
    public class ProductCategory : EntityBase
    {
        // Required for EF
        private ProductCategory()
        {
        }

        internal ProductCategory(ProductId productId, CategoryId categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }

        public ProductId ProductId { get; private set; }
        public CategoryId CategoryId { get; private set; }
        
        // Navigation property--
        public Product Product { get; set; }

        // Navigation property
        public Category Category { get; set; }

        internal static ProductCategory Create(ProductId productId, CategoryId categoryId)
        {
            return new ProductCategory(productId, categoryId);
        }
    }
}