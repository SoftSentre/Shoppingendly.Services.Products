﻿// Copyright 2020 SoftSentre Contributors
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
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Producers;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Globals.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects
{
    public class ProductProducer : ValueObject
    {
        private ProductProducer(string name)
        {
            Name = ValidateProducer(name);
        }

        public string Name { get; }

        private static string ValidateProducer(string name)
        {
            if (IsProductProducerRequired && name.IsEmpty())
            {
                throw new ProductProducerNameCanNotBeEmptyException();
            }

            if (name.IsLongerThan(ProductProducerMaxLength))
            {
                throw new ProductProducerNameIsTooLongException(ProductProducerMaxLength);
            }

            if (name.IsShorterThan(ProductProducerMinLength))
            {
                throw new ProductProducerNameIsTooShortException(ProductProducerMinLength);
            }

            return name;
        }

        public static ProductProducer Create(string name)
        {
            return new ProductProducer(name);
        }
    }
}