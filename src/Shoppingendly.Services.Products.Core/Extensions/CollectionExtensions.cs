using System.Collections.Generic;
using System.Linq;

namespace Shoppingendly.Services.Products.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> value) 
            => value == null || !value.Any();
        
        public static bool IsNotEmpty<T>(this IEnumerable<T> value) 
            => value != null && value.Any();
        
        public static bool IsEmpty<T>(this List<T> value) 
            => value == null || !value.Any();
        
        public static bool IsNotEmpty<T>(this List<T> value) 
            => value != null && value.Any();
    }
}