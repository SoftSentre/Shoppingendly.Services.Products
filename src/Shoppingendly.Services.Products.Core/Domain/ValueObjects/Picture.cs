using Shoppingendly.Services.Products.Core.Domain.Base.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Products;
using Shoppingendly.Services.Products.Core.Extensions;

namespace Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class Picture : ValueObject<Picture>
    {
        private bool _isEmpty;

        public string Name { get; private set; }
        public string Url { get; private set; }

        public bool IsEmpty => _isEmpty = Name.IsEmpty() || Url.IsEmpty();

        private Picture()
        {
            // Required for EF
        }

        public Picture(string name, string url)
        {
            Name = ValidatePictureName(name);
            Url = ValidatePictureUrl(url);
        }

        private static string ValidatePictureName(string name)
        {
            if (name.IsEmpty())
                throw new PictureNameCanNotBeEmptyException("Picture name can not be empty.");
            if (name.IsLongerThan(200))
                throw new PictureNameIsTooLongException("Picture name can not be longer than 200 characters.");

            return name;
        }

        private static string ValidatePictureUrl(string url)
        {
            if (url.IsEmpty())
                throw new PictureUrlCanNotBeEmptyException("Picture url can not be empty.");
            if (url.Contains(' '))
                throw new PictureUrlCanNotHaveWhiteSpacesException("Picture url can not have whitespaces.");
            if (url.IsLongerThan(500))
                throw new PictureUrlIsTooLongException("Picture url can not be longer than 500 characters.");

            return url;
        }
        
        public static Picture Empty => new Picture();

        public static Picture Create(string name, string url)
        {
            return new Picture(name, url);
        }

        protected override bool EqualsCore(Picture other)
        {
            return Name.Equals(other.Name) && Url.Equals(other.Url);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 13;
            hash = hash * 7 + Name.GetHashCode();
            hash = hash * 7 + Url.GetHashCode();
            
            return hash;
        }
    }
}