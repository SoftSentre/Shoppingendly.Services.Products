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

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class CreatorEfRepository : ICreatorRepository
    {
        private readonly ProductServiceDbContext _productServiceDbContext;

        public CreatorEfRepository(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Creator>> GetByIdAsync(CreatorId id)
        {
            return await _productServiceDbContext.Creators.FirstOrDefaultAsync(p => p.CreatorId.Equals(id));
        }

        public async Task<Maybe<Creator>> GetByNameAsync(string name)
        {
            return await _productServiceDbContext.Creators.FirstOrDefaultAsync(p => p.CreatorName == name);
        }

        public async Task<Maybe<Creator>> GetWithIncludesAsync(CreatorId id)
        {
            return await _productServiceDbContext.Creators.Include(c => c.Products)
                .FirstOrDefaultAsync(p => p.CreatorId.Equals(id));
        }

        public async Task AddAsync(Creator creator)
        {
            await _productServiceDbContext.AddAsync(creator);
        }

        public void Update(Creator creator)
        {
            _productServiceDbContext.Creators.Update(creator);
        }

        public void Delete(Creator creator)
        {
            _productServiceDbContext.Creators.Remove(creator);
        }
    }
}