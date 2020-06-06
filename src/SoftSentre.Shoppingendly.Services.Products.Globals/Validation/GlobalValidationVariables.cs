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

namespace SoftSentre.Shoppingendly.Services.Products.Globals.Validation
{
    /// <summary>
    ///     Central point in application with configuration for validation.
    ///     I'm decided to store the validation values in one place in application,
    ///     this approach provide more consistency for values used during the validations.
    /// </summary>
    public static class GlobalValidationVariables
    {
        // Product
        public const int ProductNameMinLength = 4;
        public const int ProductNameMaxLength = 30;
        public const bool IsProductNameRequired = true;
        public const int ProductProducerMinLength = 2;
        public const int ProductProducerMaxLength = 50;
        public const bool IsProductProducerRequired = true;

        // Creator
        public const int CreatorNameMinLength = 3;
        public const int CreatorNameMaxLength = 50;
        public const bool IsCreatorNameRequired = true;
        public const int CreatorEmailMinLength = 8;
        public const int CreatorEmailMaxLength = 100;
        public const bool IsCreatorEmailRequired = true;

        // Category
        public const int CategoryNameMinLength = 4;
        public const int CategoryNameMaxLength = 30;
        public const bool IsCategoryNameRequired = true;
        public const int CategoryDescriptionMinLength = 20;
        public const int CategoryDescriptionMaxLength = 4000;
        public const bool IsCategoryDescriptionRequired = false;

        // Role
        public const int RoleNameMaxLength = 50;

        // Picture
        public const int PictureNameMaxLength = 200;
        public const int PictureUrlMaxLength = 500;
    }
}