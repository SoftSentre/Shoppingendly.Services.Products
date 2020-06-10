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
    public class Picture : ValueObject
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
                throw new PictureNameCanNotBeEmptyException();
            }

            if (name.IsLongerThan(PictureNameMaxLength))
            {
                throw new PictureNameIsTooLongException(PictureNameMaxLength);
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

            if (url.IsLongerThan(PictureUrlMaxLength))
            {
                throw new PictureUrlIsTooLongException(PictureUrlMaxLength);
            }

            return url;
        }

        public static Picture Create(string name, string url)
        {
            return new Picture(name, url);
        }
    }
}