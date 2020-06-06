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
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Products;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using static SoftSentre.Shoppingendly.Services.Products.Globals.Validation.GlobalValidationVariables;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects
{
    public class ProductProducer : ValueObject<ProductProducer>
    {
        public string Name { get; }

        private ProductProducer(string name)
        {
            Name = ValidateProducer(name);
        }
        
        private static string ValidateProducer(string name)
        {
            if (IsProductProducerRequired && name.IsEmpty())
            {
                throw new InvalidProductProducerException("Product producer can not be empty.");
            }

            if (name.IsLongerThan(ProductProducerMaxLength))
            {
                throw new InvalidProductProducerException(
                    $"Product producer can not be longer than {ProductProducerMaxLength} characters.");
            }

            if (name.IsShorterThan(ProductProducerMinLength))
            {
                throw new InvalidProductProducerException(
                    $"Product producer can not be shorter than {ProductProducerMinLength} characters.");
            }

            return name;
        }

        public static ProductProducer CreateProductProducer(string name) => new ProductProducer(name);

        protected override bool EqualsCore(ProductProducer other)
        {
            return Name.Equals(other.Name);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 13;
            hash = hash * 7 + Name.GetHashCode();
            
            return hash;
        }
    }
}