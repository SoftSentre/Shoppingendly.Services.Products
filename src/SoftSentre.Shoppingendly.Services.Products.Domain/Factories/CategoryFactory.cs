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

using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Categories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Factories
{
    public class CategoryFactory
    {
        private readonly ICategoryBusinessRulesChecker _categoryBusinessRulesChecker;
        private readonly IDomainEventEmitter _domainEventEmitter;

        internal CategoryFactory(ICategoryBusinessRulesChecker categoryBusinessRulesChecker,
            IDomainEventEmitter domainEventEmitter)
        {
            _categoryBusinessRulesChecker = categoryBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _domainEventEmitter = domainEventEmitter.IfEmptyThenThrowAndReturnValue();
        }

        internal Category Create(CategoryId categoryId, string categoryName)
        {
            CheckBusinessRules(categoryId, categoryName);

            var category = new Category(categoryId, categoryName);
            _domainEventEmitter.Emit(category,
                new NewCategoryCreatedDomainEvent(categoryId, categoryName, Picture.Empty));
            return category;
        }

        internal Category Create(CategoryId categoryId, CategoryId parentCategoryId, string categoryName)
        {
            CheckBusinessRules(categoryId, categoryName, parentCategoryId: parentCategoryId);

            var category = new Category(categoryId, parentCategoryId, categoryName);
            _domainEventEmitter.Emit(category,
                new NewCategoryCreatedDomainEvent(categoryId, parentCategoryId, categoryName, Picture.Empty));
            return category;
        }

        internal Category Create(CategoryId categoryId, string categoryName, Picture categoryIcon)
        {
            CheckBusinessRules(categoryId, categoryName, categoryIcon: categoryIcon);

            var category = new Category(categoryId, categoryName, categoryIcon);
            _domainEventEmitter.Emit(category,
                new NewCategoryCreatedDomainEvent(categoryId, categoryName, categoryIcon));
            return category;
        }

        internal Category Create(CategoryId categoryId, CategoryId parentCategoryId, string categoryName,
            Picture categoryIcon)
        {
            CheckBusinessRules(categoryId, categoryName, categoryIcon: categoryIcon,
                parentCategoryId: parentCategoryId);

            var category = new Category(categoryId, parentCategoryId, categoryName, categoryIcon);
            _domainEventEmitter.Emit(category,
                new NewCategoryCreatedDomainEvent(categoryId, parentCategoryId, categoryName, categoryIcon));
            return category;
        }

        internal Category Create(CategoryId categoryId, string categoryName, string categoryDescription)
        {
            CheckBusinessRules(categoryId, categoryName, categoryDescription);

            var category = new Category(categoryId, categoryName, categoryDescription);
            _domainEventEmitter.Emit(category, new NewCategoryCreatedDomainEvent(categoryId, categoryName,
                categoryDescription, Picture.Empty));
            return category;
        }

        internal Category Create(CategoryId categoryId, CategoryId parentCategoryId, string categoryName,
            string categoryDescription)
        {
            CheckBusinessRules(categoryId, categoryName, categoryDescription, parentCategoryId: parentCategoryId);

            var category = new Category(categoryId, parentCategoryId, categoryName, categoryDescription);
            _domainEventEmitter.Emit(category, new NewCategoryCreatedDomainEvent(categoryId, parentCategoryId,
                categoryName,
                categoryDescription, Picture.Empty));
            return category;
        }

        internal Category Create(CategoryId categoryId, string categoryName, string categoryDescription,
            Picture categoryIcon)
        {
            CheckBusinessRules(categoryId, categoryName, categoryDescription, categoryIcon);

            var category = new Category(categoryId, categoryName, categoryDescription, categoryIcon);
            _domainEventEmitter.Emit(category,
                new NewCategoryCreatedDomainEvent(categoryId, categoryName, categoryDescription, categoryIcon));
            return category;
        }

        internal Category Create(CategoryId categoryId, CategoryId parentCategoryId, string categoryName,
            string categoryDescription,
            Picture categoryIcon)
        {
            CheckBusinessRules(categoryId, categoryName, categoryDescription, categoryIcon, parentCategoryId);

            var category = new Category(categoryId, parentCategoryId, categoryName, categoryDescription, categoryIcon);
            _domainEventEmitter.Emit(category,
                new NewCategoryCreatedDomainEvent(categoryId, parentCategoryId, categoryName, categoryDescription,
                    categoryIcon));
            return category;
        }

        private void CheckBusinessRules(CategoryId categoryId, string categoryName, string categoryDescription = null,
            Picture categoryIcon = null, CategoryId parentCategoryId = null)
        {
            if (_categoryBusinessRulesChecker.CategoryIdCanNotBeEmptyRuleIsBroken(categoryId))
                throw new InvalidCategoryIdException(categoryId);
            if (parentCategoryId != null &&
                _categoryBusinessRulesChecker.ParentCategoryIdMustBeValidWhenProvidedRuleIsBroken(parentCategoryId))
                throw new InvalidParentCategoryIdException(parentCategoryId);
            if (_categoryBusinessRulesChecker.CategoryNameCanNotBeEmptyRuleIsBroken(categoryName))
                throw new CategoryNameCanNotBeEmptyException();
            if (_categoryBusinessRulesChecker.CategoryNameCanNotBeShorterThanRuleIsBroken(categoryName))
                throw new CategoryNameIsTooShortException(GlobalValidationVariables.CategoryNameMinLength);
            if (_categoryBusinessRulesChecker.CategoryNameCanNotBeLongerThanRuleIsBroken(categoryName))
                throw new CategoryNameIsTooLongException(GlobalValidationVariables.CategoryNameMaxLength);
            if (categoryDescription.IsNotEmpty() &&
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeEmptyRuleIsBroken(categoryDescription))
                throw new CategoryDescriptionCanNotBeEmptyException();
            if (categoryDescription.IsNotEmpty() &&
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeShorterThanRuleIsBroken(categoryDescription))
                throw new CategoryDescriptionIsTooShortException(GlobalValidationVariables
                    .CategoryDescriptionMinLength);
            if (categoryDescription.IsNotEmpty() &&
                _categoryBusinessRulesChecker.CategoryDescriptionCanNotBeLongerThanRuleIsBroken(categoryDescription))
                throw new CategoryDescriptionIsTooLongException(GlobalValidationVariables.CategoryDescriptionMaxLength);
            if (categoryIcon != null &&
                _categoryBusinessRulesChecker.CategoryIconCanNotBeNullOrEmptyRuleIsBroken(categoryIcon))
                throw new CategoryIconCanNotBeNullOrEmptyException();
        }
    }
}