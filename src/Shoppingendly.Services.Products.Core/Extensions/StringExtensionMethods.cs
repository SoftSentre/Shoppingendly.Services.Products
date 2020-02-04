using System.Collections.Generic;
using System.Linq;

namespace Shoppingendly.Services.Products.Core.Extensions
{
    public static class StringExtensionMethods
    {
        public static bool IsEmpty(this string target) => string.IsNullOrWhiteSpace(target);

        public static bool IsNotEmpty(this string target) => !target.IsEmpty();

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