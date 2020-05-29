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
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Core.Types;

namespace SoftSentre.Shoppingendly.Services.Products.Core.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Maybe<Category>> GetByIdAsync(CategoryId categoryId);
        Task<Maybe<Category>> GetByNameAsync(string name);
        Task<Maybe<Category>> GetByNameWithIncludesAsync(string name);
        Task<Maybe<IEnumerable<Category>>> GetAllAsync();
        Task<Maybe<IEnumerable<Category>>> GetAllWithIncludesAsync();
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}