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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class ProductEfRepository : IProductRepository
    {
        private readonly ProductServiceDbContext _productServiceDbContext;

        public ProductEfRepository(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Product>> GetByIdAsync(ProductId id)
        {
            return await _productServiceDbContext.Products.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<Maybe<Product>> GetByIdWithIncludesAsync(ProductId productId)
        {
            return await _productServiceDbContext.Products.Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id.Equals(productId));
        }

        public async Task<Maybe<IEnumerable<Product>>> GetManyByNameAsync(string name)
        {
            return await _productServiceDbContext.Products
                .Where(p => p.ProductName == name).ToListAsync();
        }

        public async Task<Maybe<IEnumerable<Product>>> GetManyByNameWithIncludesAsync(string name)
        {
            return await _productServiceDbContext.Products.Include(p => p.ProductCategories)
                .Where(p => p.ProductName == name).ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _productServiceDbContext.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _productServiceDbContext.Update(product);
        }

        public void Delete(Product product)
        {
            _productServiceDbContext.Remove(product);
        }
    }
}