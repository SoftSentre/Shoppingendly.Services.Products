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
    public static class ErrorCodes
    {
        // Products
        public const string ProductNameCanNotBeEmpty = "product_name_can_not_be_empty";
        public const string ProductNameIsTooShort = "product_name_is_too_short";
        public const string ProductNameIsTooLong = "product_name_is_too_long";
        public const string ProductIsAlreadyAssignedToCategory = "product_already_assigned_to_category";
        public const string ProductWithAssignedCategoryNotFound = "product_with_assigned_category_not_found";
        public const string ProductWithAssignedCategoriesNotFound = "product_with_assigned_categories_not_found";
        public const string ProductPictureCanNotBeNullOrEmpty = "product_picture_can_not_be_null_or_empty";
        public const string InvalidProductId = "invalid_product_id";
        public const string CreateProductFailed = "create_product_failed";
        public const string ProductAlreadyExists = "product_already_exist";
        public const string ProductNotFound = "product_not_found";

        // Categories
        public const string CategoryNameCanNotBeEmpty = "category_name_can_not_be_empty";
        public const string CategoryNameIsTooShort = "category_name_is_too_short";
        public const string CategoryNameIsTooLong = "category_name_is_too_long";
        public const string CategoryDescriptionCanNotBeEmpty = "category_description_can_not_be_empty";
        public const string CategoryDescriptionIsTooShort = "category_description_is_too_long";
        public const string CategoryDescriptionIsTooLong = "category_description_is_too_short";
        public const string CategoryIconCanNotBeNullOrEmpty = "category_icon_can_not_be_null_or_empty";
        public const string InvalidCategoryId = "invalid_category_id";
        public const string CreateCategoryFailed = "create_category_failed";
        public const string CategoryAlreadyExists = "category_already_exists";
        public const string CategoryNotFound = "category_not_found";

        // Creators
        public const string CreatorNameCanNotBeEmpty = "creator_name_can_not_be_empty";
        public const string CreatorNameIsTooShort = "creator_name_is_too_short";
        public const string CreatorNameIsTooLong = "creator_name_is_too_long";
        public const string RoleIsTooLong = "role_is_too_long";
        public const string CreatorAlreadyExists = "creator_already_exist";
        public const string CreateCreatorFailed = "create_creator_failed";
        public const string InvalidCreatorId = "invalid_creator_id";
        public const string CreatorNotFound = "creator_not_found";

        // Producers
        public const string ProductProducerNameCanNotBeEmpty = "product_producer_name_can_not_be_empty";
        public const string ProductProducerCanNotBeNull = "product_producer_can_not_be_null";
        public const string ProductProducerNameIsTooShort = "product_producer_name_is_too_short";
        public const string ProductProducerNameIsTooLong = "product_producer_name_is_too_long";

        // Picture
        public const string PictureNameCanNotBeEmpty = "picture_name_can_not_be_empty";
        public const string PictureNameIsTooLong = "picture_name_is_too_long";
        public const string PictureUrlCanNotBeEmpty = "picture_url_can_not_be_empty";
        public const string PictureUrlCanNotContainsWhitespaces = "picture_url_can_not_contains_whitespaces";
        public const string PictureUrlIsTooLong = "picture_url_is_too_long";

        // Domain events
        public const string DispatchDomainEventsFailed = "dispatch_domain_events_failed";
        public const string DomainEventCanNotBeEmpty = "domain_event_can_not_be_empty";
        public const string PublishDomainEventFailed = "publish_domain_event_failed";
        public const string EmitDomainEventFailed = "emit_domain_event_failed";
        public const string ClearDomainEventsFailed = "clear_domain_events_failed";
        public const string GetUncommittedDomainEventsFailed = "get_uncommitted_domain_events_faileds";

        // Commands
        public const string CommandPublishedFailed = "command_published_failed";
        public const string InvalidCommand = "invalid_command";
        public const string SendingQueryFailed = "sending_query_failed";
    }
}