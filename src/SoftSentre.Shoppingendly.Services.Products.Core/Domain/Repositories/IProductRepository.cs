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
using System.Threading.Tasks;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects;

namespace SoftSentre.Shoppingendly.Services.Products.Core.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Maybe<Product>> GetByIdAsync(ProductId productId);
        Task<Maybe<Product>> GetByIdWithIncludesAsync(ProductId productId);
        Task<Maybe<IEnumerable<Product>>> GetManyByNameAsync(string name);
        Task<Maybe<IEnumerable<Product>>> GetManyByNameWithIncludesAsync(string name);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}