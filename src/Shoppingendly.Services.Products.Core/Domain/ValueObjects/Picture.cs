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

        protected Picture()
        {
            // Required for EF
        }

        public Picture(string name, string url)
        {
            if (name.IsEmpty())
                throw new PictureCanNotBeEmptyException("Avatar name can not be empty.");
            
            if (url.IsEmpty())
                throw new PictureCanNotBeEmptyException("Avatar Url can not be empty.");
            
            Name = name;
            Url = url;
        }

        public static Picture Empty => new Picture();

        public static Picture Create(string name, string url)
        {
            return new Picture(name, url);
        }

        protected override bool EqualsCore(Picture other)
        {
            return Name.Equals(other.Name);
        }

        protected override int GetHashCodeCore()
        {
            return Name.GetHashCode();
        }
    }
}