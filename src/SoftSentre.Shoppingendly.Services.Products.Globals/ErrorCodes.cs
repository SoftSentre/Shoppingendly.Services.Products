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

namespace SoftSentre.Shoppingendly.Services.Products.Globals
{
    public class ErrorCodes
    {
        // Products
        public static readonly string ProductNameCanNotBeEmpty = "product_name_can_not_be_empty";
        public static readonly string ProductNameIsTooShort = "product_name_is_too_short";
        public static readonly string ProductNameIsTooLong = "product_name_is_too_long";
        public static readonly string ProductIsAlreadyAssignedToCategory = "product_already_assigned_to_category";
        public static readonly string ProductWithAssignedCategoryNotFound = "product_with_assigned_category_not_found";

        public static readonly string ProductWithAssignedCategoriesNotFound =
            "product_with_assigned_categories_not_found";

        public static readonly string PictureCanNotBeNullOrEmpty = "picture_can_not_be_null_or_empty";
        public static readonly string ProductAlreadyExists = "product_already_exist";
        public static readonly string ProductNotFound = "product_not_found";

        // Categories
        public static readonly string CategoryNameCanNotBeEmpty = "category_name_can_not_be_empty";
        public static readonly string CategoryNameIsTooShort = "category_name_is_too_short";
        public static readonly string CategoryNameIsTooLong = "category_name_is_too_long";
        public static readonly string CategoryDescriptionCanNotBeEmpty = "category_description_can_not_be_empty";
        public static readonly string CategoryDescriptionIsTooShort = "category_description_is_too_long";
        public static readonly string CategoryDescriptionIsTooLong = "category_description_is_too_short";
        public static readonly string CategoryAlreadyExists = "category_already_exists";
        public static readonly string CategoryNotFound = "category_not_found";

        // Creators
        public static readonly string CreatorNameCanNotBeEmpty = "creator_name_can_not_be_empty";
        public static readonly string CreatorNameIsTooShort = "creator_name_is_too_short";
        public static readonly string CreatorNameIsTooLong = "creator_name_is_too_long";
        public static readonly string RoleIsTooLong = "role_is_too_long";
        public static readonly string CreatorAlreadyExists = "creator_already_exist";
        public static readonly string CreatorNotFound = "creator_not_found";

        // Producers
        public static readonly string ProductProducerNameCanNotBeEmpty = "product_producer_name_can_not_be_empty";
        public static readonly string ProductProducerCanNotBeNull = "product_producer_can_not_be_null";
        public static readonly string ProductProducerNameIsTooShort = "product_producer_name_is_too_short";
        public static readonly string ProductProducerNameIsTooLong = "product_producer_name_is_too_long";

        // Picture
        public static readonly string PictureNameCanNotBeEmpty = "picture_name_can_not_be_empty";
        public static readonly string PictureNameIsTooLong = "picture_name_is_too_long";
        public static readonly string PictureUrlCanNotBeEmpty = "picture_url_can_not_be_empty";
        public static readonly string PictureUrlCanNotContainsWhitespaces = "picture_url_can_not_contains_whitespaces";
        public static readonly string PictureUrlIsTooLong = "picture_url_is_too_long";
    }
}