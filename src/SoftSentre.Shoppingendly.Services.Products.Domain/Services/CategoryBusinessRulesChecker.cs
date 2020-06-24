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

using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Services
{
    public class CategoryBusinessRulesChecker : ICategoryBusinessRulesChecker
    {
        public bool CategoryIdCanNotBeEmptyRuleIsBroken(CategoryId categoryId)
        {
            return !categoryId.IsValid();
        }

        public bool CategoryNameCanNotBeEmptyRuleIsBroken(string categoryName)
        {
            return GlobalValidationVariables.IsCategoryNameRequired && categoryName.IsEmpty();
        }

        public bool CategoryNameCanNotBeShorterThanRuleIsBroken(string categoryName)
        {
            return categoryName.IsShorterThan(GlobalValidationVariables.CategoryNameMinLength);
        }

        public bool CategoryNameCanNotBeLongerThanRuleIsBroken(string categoryName)
        {
            return categoryName.IsLongerThan(GlobalValidationVariables.CategoryNameMaxLength);
        }

        public bool CategoryDescriptionCanNotBeEmptyRuleIsBroken(string categoryDescription)
        {
            return GlobalValidationVariables.IsCategoryDescriptionRequired && categoryDescription.IsEmpty();
        }

        public bool CategoryDescriptionCanNotBeShorterThanRuleIsBroken(string categoryDescription)
        {
            return categoryDescription.IsShorterThan(GlobalValidationVariables.CategoryDescriptionMinLength);
        }

        public bool CategoryDescriptionCanNotBeLongerThanRuleIsBroken(string categoryDescription)
        {
            return categoryDescription.IsLongerThan(GlobalValidationVariables.CategoryDescriptionMaxLength);
        }

        public bool CategoryIconCanNotBeNullOrEmptyRuleIsBroken(Maybe<Picture> categoryIcon)
        {
            return categoryIcon.HasNoValue || categoryIcon.Value.IsEmpty;
        }
    }
}