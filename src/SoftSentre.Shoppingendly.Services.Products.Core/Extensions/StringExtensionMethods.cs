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

using System.Collections.Generic;
using System.Linq;

namespace SoftSentre.Shoppingendly.Services.Products.Core.Extensions
{
    public static class StringExtensionMethods
    {
        public static bool IsEmpty(this string target) => string.IsNullOrWhiteSpace(target);

        public static bool IsNotEmpty(this string target) => !target.IsEmpty();

        public static string CreateStringWithSpecificNumberOfCharacters(int length) 
            => new string('*', length);
        
        public static string TrimToUpper(this string value)
        {
            return value.OrEmpty().Trim().ToUpperInvariant();
        }

        public static string TrimToLower(this string value)
        {
            return value.OrEmpty().Trim().ToLowerInvariant();
        }

        public static string OrEmpty(this string value)
        {
            return value.IsEmpty() ? "" : value;
        }

        public static bool IsLongerThan(this string value, int numberOfCharacters)
            => value.Length > numberOfCharacters;

        public static bool IsShorterThan(this string value, int numberOfCharacters)
            => value.Length < numberOfCharacters;

        public static bool EqualsCaseInvariant(this string value, string valueToCompare)
        {
            if (value.IsEmpty())
                return valueToCompare.IsEmpty();
            if (valueToCompare.IsEmpty())
                return false;

            var fixedValue = value.TrimToUpper();
            var fixedValueToCompare = valueToCompare.TrimToUpper();

            return fixedValue == fixedValueToCompare;
        }

        public static string Underscore(this string value)
            => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));

        public static string AggregateLines(this IEnumerable<string> values)
            => values.Aggregate((x, y) => $"{x.Trim()}\n{y.Trim()}");
    }
}