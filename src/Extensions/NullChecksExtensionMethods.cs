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

using System;

namespace SoftSentre.Shoppingendly.Services.Products.Extensions
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

        public static bool IfEmptyThenThrowOrReturnBool<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            return true;
        }

        public static bool IfEmptyThenThrowOrReturnBool<T>(this T value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name, message);
            }

            return true;
        }

        public static T IfEmptyThenThrowOrReturnValue<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name);
            }

            return value;
        }

        public static T IfEmptyThenThrowOrReturnValue<T>(this T value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(typeof(T).Name, message);
            }

            return value;
        }
    }
}