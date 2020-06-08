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
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Services
{
    public class CategoryDomainService : ICategoryDomainService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryDomainService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Category>> GetCategoryAsync(CategoryId categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            return category;
        }

        public async Task<Maybe<Category>> GetCategoryByNameAsync(string categoryName)
        {
            var category = await _categoryRepository.GetByNameAsync(categoryName);
            return category;
        }

        public async Task<Maybe<Category>> GetCategoryWithProductsAsync(string categoryName)
        {
            var category = await _categoryRepository.GetByNameWithIncludesAsync(categoryName);
            return category;
        }

        public async Task<Maybe<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            var category = await _categoryRepository.GetAllAsync();
            return category;
        }

        public async Task<Maybe<IEnumerable<Category>>> GetAllCategoriesWithProductsAsync()
        {
            var category = await _categoryRepository.GetAllWithIncludesAsync();
            return category;
        }

        public async Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, string categoryName)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category.HasValue)
            {
                throw new CategoryAlreadyExistsException(categoryId);
            }

            var newCategory = Category.Create(categoryId, categoryName);
            await _categoryRepository.AddAsync(newCategory);

            return newCategory;
        }

        public async Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, string categoryName,
            string description)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category.HasValue)
            {
                throw new CategoryAlreadyExistsException(categoryId);
            }

            var newCategory = Category.Create(categoryId, categoryName, description);
            await _categoryRepository.AddAsync(newCategory);

            return newCategory;
        }

        public async Task<bool> SetCategoryNameAsync(CategoryId categoryId, string categoryName)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId)
                .UnwrapAsync(new CategoryNotFoundException(categoryId));

            var isNameChanged = category.SetCategoryName(categoryName);

            if (isNameChanged)
            {
                _categoryRepository.Update(category);
            }

            return isNameChanged;
        }

        public async Task<bool> SetCategoryDescriptionAsync(CategoryId categoryId, string categoryDescription)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId)
                .UnwrapAsync(new CategoryNotFoundException(categoryId));

            var isDescriptionChanged = category.SetCategoryDescription(categoryDescription);

            if (isDescriptionChanged)
            {
                _categoryRepository.Update(category);
            }

            return isDescriptionChanged;
        }
    }
}