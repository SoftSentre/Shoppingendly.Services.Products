namespace Shoppingendly.Services.Products.Core.Validation
{
    /// <summary>
    /// Central point in application with configuration for validation.
    /// I'm decided to store the validation values in one place in application,
    /// this approach provide more consistency for values used during the validations.
    /// It could be part of the appSettings.json file in future.
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