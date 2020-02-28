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
        public static readonly int ProductNameMinLength = 4;
        public static readonly int ProductNameMaxLength = 30;
        public static readonly bool IsProductNameRequired = true;
        public static readonly int ProductProducerMinLength = 2;
        public static readonly int ProductProducerMaxLength = 50;
        public static readonly bool IsProductProducerRequired = true;
        
        // Creator
        public static readonly int CreatorNameMinLength = 3;
        public static readonly int CreatorNameMaxLength = 50;
        public static readonly bool IsCreatorNameRequired = true;
        public static readonly int CreatorEmailMinLength = 8;
        public static readonly int CreatorEmailMaxLength = 100;
        public static readonly bool IsCreatorEmailRequired = true;
        
        // Category
        public static readonly int CategoryNameMinLength = 4;
        public static readonly int CategoryNameMaxLength = 30;
        public static readonly bool IsCategoryNameRequired = true;
        public static readonly int CategoryDescriptionMinLength = 20;
        public static readonly int CategoryDescriptionMaxLength = 4000;
        public static readonly bool IsCreatorDescriptionRequired = false;

        // Role
        public static readonly int RoleNameMaxLength = 50;
        
        // Picture
        public static readonly int PictureNameMaxLength = 200;
        public static readonly int PictureUrlMaxLength = 500;
    }
}