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
using System.Threading.Tasks;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;

namespace SoftSentre.Shoppingendly.Services.Products.Extensions
{
    public static class MaybeExtensionMethods
    {
        public static T Unwrap<T>(this Maybe<T> maybe, T defaultValue = null) where T : class
        {
            return maybe.Unwrap(x => x, defaultValue);
        }

        public static TK Unwrap<T, TK>(this Maybe<T> maybe, Func<T, TK> selector, TK defaultValue = default)
            where T : class
        {
            return maybe.HasValue ? selector(maybe.Value) : defaultValue;
        }

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate) where T : class
        {
            if (maybe.HasNoValue)
            {
                return null;
            }

            return predicate(maybe.Value) ? maybe : null;
        }

        public static Maybe<TK> Select<T, TK>(this Maybe<T> maybe, Func<T, TK> selector)
            where T : class where TK : class
        {
            return maybe.HasNoValue ? null : selector(maybe.Value);
        }

        public static Maybe<TK> Select<T, TK>(this Maybe<T> maybe, Func<T, Maybe<TK>> selector) where T : class
            where TK : class
        {
            return maybe.HasNoValue ? null : selector(maybe.Value);
        }

        public static void Execute<T>(this Maybe<T> maybe, Action<T> action) where T : class
        {
            if (maybe.HasNoValue)
            {
                return;
            }

            action(maybe.Value);
        }

        public static async Task<T> UnwrapAsync<T>(this Task<Maybe<T>> maybe, Exception noValueException = null)
            where T : class
        {
            var result = await maybe;

            if (result.HasValue)
            {
                return result.Value;
            }

            if (noValueException == null)
            {
                return default;
            }

            throw noValueException;
        }
    }
}