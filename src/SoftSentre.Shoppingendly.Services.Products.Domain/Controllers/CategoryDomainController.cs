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
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Controllers
{
    public class CategoryDomainController : ICategoryDomainController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryBusinessRulesChecker _categoryBusinessRulesChecker;
        private readonly IDomainEventEmitter _domainEventEmitter;
        private readonly CategoryFactory _categoryFactory;

        public CategoryDomainController(ICategoryRepository categoryRepository,
            ICategoryBusinessRulesChecker categoryBusinessRulesChecker, CategoryFactory categoryFactory,
            IDomainEventEmitter domainEventEmitter)
        {
            _categoryRepository = categoryRepository.IfEmptyThenThrowAndReturnValue();
            _categoryBusinessRulesChecker = categoryBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _domainEventEmitter = domainEventEmitter.IfEmptyThenThrowAndReturnValue();
            _categoryFactory = categoryFactory.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Category>> GetCategoryByIdAsync(CategoryId categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
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
            return await CreateCategoryAsync(categoryId, categoryName);
        }

        public async Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, string categoryName,
            Picture categoryIcon)
        {
            return await CreateCategoryAsync(categoryId, categoryName, categoryIcon: categoryIcon);
        }

        public async Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, string categoryName,
            string categoryDescription)
        {
            return await CreateCategoryAsync(categoryId, categoryName, categoryDescription);
        }

        public async Task<Maybe<Category>> CreateNewCategoryAsync(CategoryId categoryId, string categoryName,
            string categoryDescription, Picture categoryIcon)
        {
            return await CreateCategoryAsync(categoryId, categoryName, categoryDescription, categoryIcon);
        }

        public async Task<bool> ChangeCategoryNameAsync(CategoryId categoryId, string categoryName)
        {
            var category = await _categoryRepository.GetByIdAndThrowIfEntityNotFound(categoryId, new CategoryNotFoundException(categoryId));

            if (_categoryBusinessRulesChecker.CategoryNameCanNotBeEmptyRuleIsBroken(categoryName))
                throw new CategoryNameCanNotBeEmptyException();
            if (_categoryBusinessRulesChecker.CategoryNameCanNotBeShorterThanRuleIsBroken(categoryName))
                throw new CategoryNameIsTooShortException(GlobalValidationVariables.CategoryNameMinLength);
            if (_categoryBusinessRulesChecker.CategoryNameCanNotBeLongerThanRuleIsBroken(categoryName))
                throw new CategoryNameIsTooLongException(GlobalValidationVariables.CategoryNameMaxLength);

            var isNameChanged = category.ChangeCategoryName(categoryName);

            _domainEventEmitter.Emit(category, new CategoryNameChangedDomainEvent(categoryId, categoryName));

            if (isNameChanged)
            {
                _categoryRepository.Update(category);
            }

            return isNameChanged;
        }

        public async Task<bool> ChangeCategoryDescriptionAsync(CategoryId categoryId, string categoryDescription)
        {
            var category = await _categoryRepository.GetByIdAndThrowIfEntityNotFound(categoryId, new CategoryNotFoundException(categoryId));

            if (_categoryBusinessRulesChecker.CategoryDescriptionCanNotBeEmptyRuleIsBroken(categoryDescription))
                throw new CategoryDescriptionCanNotBeEmptyException();
            if (_categoryBusinessRulesChecker.CategoryDescriptionCanNotBeShorterThanRuleIsBroken(categoryDescription))
                throw new CategoryDescriptionIsTooShortException(GlobalValidationVariables
                    .CategoryDescriptionMinLength);
            if (_categoryBusinessRulesChecker.CategoryDescriptionCanNotBeLongerThanRuleIsBroken(categoryDescription))
                throw new CategoryDescriptionIsTooLongException(GlobalValidationVariables.CategoryDescriptionMaxLength);

            var isDescriptionChanged =
                category.SetCategoryDescription(categoryDescription);

            _domainEventEmitter.Emit(category,
                new CategoryDescriptionChangedDomainEvent(categoryId, categoryDescription));

            if (isDescriptionChanged)
            {
                _categoryRepository.Update(category);
            }

            return isDescriptionChanged;
        }

        public async Task<bool> UploadCategoryIconAsync(CategoryId categoryId, Picture categoryIcon)
        {
            var category = await _categoryRepository.GetByIdAndThrowIfEntityNotFound(categoryId, new CategoryNotFoundException(categoryId));

            if (_categoryBusinessRulesChecker.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(categoryIcon))
                throw new CategoryIconCanNotBeNullOrEmptyException();

            var isCategoryIconChanged = category.UploadCategoryIcon(categoryIcon);

            _domainEventEmitter.Emit(category, new CategoryIconUploadedDomainEvent(categoryId, categoryIcon));

            if (isCategoryIconChanged)
            {
                _categoryRepository.Update(category);
            }

            return isCategoryIconChanged;
        }

        private async Task<Maybe<Category>> CreateCategoryAsync(CategoryId categoryId, string categoryName,
            string categoryDescription = null, Picture categoryIcon = null)
        {
            await _categoryRepository.GetByIdAndThrowIfEntityAlreadyExists(categoryId,
                new CategoryAlreadyExistsException(categoryId));
            
            Category newCategory;

            if (categoryDescription.IsEmpty() && categoryIcon == null)
                newCategory = _categoryFactory.Create(categoryId, categoryName);
            else if (categoryDescription.IsEmpty() && categoryIcon != null)
                newCategory = _categoryFactory.Create(categoryId, categoryName, categoryIcon);
            else if (categoryIcon == null && categoryDescription.IsNotEmpty())
                newCategory = _categoryFactory.Create(categoryId, categoryName, categoryDescription);
            else
                newCategory =
                    _categoryFactory.Create(categoryId, categoryName, categoryDescription, categoryIcon);

            await _categoryRepository.AddAsync(newCategory);

            return newCategory;
        }
    }
}