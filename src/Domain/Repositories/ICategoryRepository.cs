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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.SeedWork;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category, CategoryId>
    {
        Task<Maybe<Category>> GetByNameAsync(string name);
        Task<Maybe<Category>> GetByNameWithIncludesAsync(string name);
        Task<Maybe<IEnumerable<Category>>> GetAllAsync();
        Task<Maybe<IEnumerable<Category>>> GetAllWithIncludesAsync();
        Task<Maybe<IEnumerable<Category>>> FindAsync(Expression<Func<Category, bool>> predicate);
        void DeleteMany(IEnumerable<Category> categories);
    }
}