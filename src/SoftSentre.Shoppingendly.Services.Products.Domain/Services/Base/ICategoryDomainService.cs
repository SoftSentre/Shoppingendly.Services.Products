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
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base
{
    public interface ICategoryDomainService
    {
        Task<Maybe<Category>> GetCategoryAsync(CategoryId categoryId);
        Task<Maybe<Category>> GetCategoryByNameAsync(string categoryName);
        Task<Maybe<Category>> GetCategoryWithProductsAsync(string categoryName);
        Task<Maybe<IEnumerable<Category>>> GetAllCategoriesAsync();
        Task<Maybe<IEnumerable<Category>>> GetAllCategoriesWithProductsAsync();
        Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, string categoryName);

        Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId,
            string categoryName, string description);

        Task<bool> SetCategoryNameAsync(CategoryId categoryId, string categoryName);
        Task<bool> SetCategoryDescriptionAsync(CategoryId categoryId, string categoryDescription);
        Task<bool> AddOrChangeCategoryIconAsync(CategoryId categoryId, Picture categoryIcon);
    }
}