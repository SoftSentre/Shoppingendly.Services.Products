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

using SoftSentre.Shoppingendly.Services.Products.Core.Domain.Base.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Core.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Core.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Core.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Core.Domain.ValueObjects
{
    public class Picture : ValueObject<Picture>
    {
        private bool _isEmpty;

        private Picture()
        {
            // Required for EF
        }

        internal Picture(string name, string url)
        {
            Name = ValidatePictureName(name);
            Url = ValidatePictureUrl(url);
        }

        public string Name { get; }
        public string Url { get; }

        public bool IsEmpty => _isEmpty = Name.IsEmpty() || Url.IsEmpty();

        public static Picture Empty => new Picture();

        private static string ValidatePictureName(string name)
        {
            if (name.IsEmpty())
            {
                throw new InvalidPictureNameException("Picture name can not be empty.");
            }

            if (name.IsLongerThan(PictureNameMaxLength))
            {
                throw new InvalidPictureNameException("Picture name can not be longer than 200 characters.");
            }

            return name;
        }

        private static string ValidatePictureUrl(string url)
        {
            if (url.IsEmpty())
            {
                throw new InvalidPictureUrlException("Picture url can not be empty.");
            }

            if (url.Contains(' '))
            {
                throw new InvalidPictureUrlException("Picture url can not have whitespaces.");
            }

            if (url.IsLongerThan(PictureUrlMaxLength))
            {
                throw new InvalidPictureUrlException("Picture url can not be longer than 500 characters.");
            }

            return url;
        }

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