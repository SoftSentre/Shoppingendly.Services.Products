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

using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Pictures;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects
{
    public class ProductPicture : ValueObject<ProductPicture>
    {
        private bool _isEmpty;

        private ProductPicture()
        {
            // Required for EF
        }

        internal ProductPicture(string name, string url)
        {
            Name = ValidatePictureName(name);
            Url = ValidatePictureUrl(url);
        }

        public string Name { get; }
        public string Url { get; }

        public bool IsEmpty => _isEmpty = Name.IsEmpty() || Url.IsEmpty();

        public static ProductPicture Empty => new ProductPicture();

        private static string ValidatePictureName(string name)
        {
            if (name.IsEmpty())
            {
                throw new PictureNameCanNotBeEmptyException();
            }

            if (name.IsLongerThan(ProductPictureNameMaxLength))
            {
                throw new PictureNameIsTooLongException(ProductPictureNameMaxLength);
            }

            return name;
        }

        private static string ValidatePictureUrl(string url)
        {
            if (url.IsEmpty())
            {
                throw new PictureUrlCanNotBeEmptyException();
            }

            if (url.Contains(' '))
            {
                throw new PictureUrlCanNotContainsWhitespacesException();
            }

            if (url.IsLongerThan(ProductPictureUrlMaxLength))
            {
                throw new PictureUrlIsTooLongException(ProductPictureUrlMaxLength);
            }

            return url;
        }

        public static ProductPicture Create(string name, string url)
        {
            return new ProductPicture(name, url);
        }

        protected override bool EqualsCore(ProductPicture other)
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