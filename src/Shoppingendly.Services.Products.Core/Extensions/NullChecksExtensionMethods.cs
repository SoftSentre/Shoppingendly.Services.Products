using System;

namespace Shoppingendly.Services.Products.Core.Extensions
{
    public static class NullChecksExtensionMethods
    {
        public static void IfEmptyThenThrow<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }
        }

        public static void IfEmptyThenThrow<T>(this T value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name, message);
            }
        }

        public static bool IfEmptyThenThrowAndReturnBool<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            return true;
        }

        public static bool IfEmptyThenThrowAndReturnBool<T>(this T value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name, message);
            }

            return true;
        }

        public static T IfEmptyThenThrowAndReturnValue<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            return value;
        }

        public static T IfEmptyThenThrowAndReturnValue<T>(this T value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name, message);
            }

            return value;
        }
    }
}