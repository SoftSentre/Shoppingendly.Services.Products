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

namespace SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types
{
    public readonly struct Maybe<T> : IEquatable<Maybe<T>> where T : class
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (HasNoValue)
                {
                    throw new InvalidOperationException();
                }

                return _value;
            }
        }

        public bool HasValue => _value != null;
        public bool HasNoValue => !HasValue;

        public Maybe(T value)
        {
            _value = value;
        }

        public static Maybe<T> Empty => new Maybe<T>();

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static bool operator ==(Maybe<T> maybe, T value)
        {
            return !maybe.HasNoValue && maybe.Value.Equals(value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj is T o)
            {
                obj = new Maybe<T>(o);
            }

            if (!(obj is Maybe<T>))
            {
                return false;
            }

            var other = (Maybe<T>) obj;

            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasNoValue && other.HasNoValue)
            {
                return true;
            }

            if (HasNoValue || other.HasNoValue)
            {
                return false;
            }

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            return HasNoValue ? 0 : _value.GetHashCode();
        }

        public override string ToString()
        {
            return HasNoValue ? "No value" : Value.ToString();
        }
    }
}