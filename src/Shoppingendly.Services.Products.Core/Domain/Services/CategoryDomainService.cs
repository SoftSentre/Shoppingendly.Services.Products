using System.Collections.Generic;
using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services.Categories;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services
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
                throw new CategoryAlreadyExistsException(
                    $"Unable to add new category, because category with id: {categoryId} is already exists.");
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
                throw new CategoryAlreadyExistsException(
                    $"Unable to add new category, because category with id: {categoryId} is already exists.");
            }

            var newCategory = Category.Create(categoryId, categoryName, description);
            await _categoryRepository.AddAsync(newCategory);

            return newCategory;
        }

        public async Task<bool> SetCategoryNameAsync(CategoryId categoryId, string categoryName)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            var validatedCategory = IfCategoryIsEmptyThenThrow(category);
            var isNameChanged = validatedCategory.SetName(categoryName);

            if (isNameChanged)
                _categoryRepository.Update(validatedCategory);

            return isNameChanged;
        }

        public async Task<bool> SetCategoryDescriptionAsync(CategoryId categoryId, string categoryDescription)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            var validatedCategory = IfCategoryIsEmptyThenThrow(category);
            var isDescriptionChanged = validatedCategory.SetDescription(categoryDescription);

            if (isDescriptionChanged)
                _categoryRepository.Update(validatedCategory);

            return isDescriptionChanged;
        }

        private static Category IfCategoryIsEmptyThenThrow(Maybe<Category> category)
        {
            if (category.HasNoValue)
            {
                throw new CategoryNotFoundException(
                    "Unable to mutate category state, because provided is empty.");
            }

            return category.Value;
        }
    }
}