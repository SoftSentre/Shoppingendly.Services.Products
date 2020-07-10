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

using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Events.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Factories
{
    public class CreatorFactory
    {
        private readonly ICreatorBusinessRulesChecker _creatorBusinessRulesChecker;
        private readonly IDomainEventEmitter _domainEventEmitter;

        internal CreatorFactory(ICreatorBusinessRulesChecker creatorBusinessRulesChecker,
            IDomainEventEmitter domainEventEmitter)
        {
            _creatorBusinessRulesChecker = creatorBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _domainEventEmitter = domainEventEmitter.IfEmptyThenThrowAndReturnValue();
        }

        internal Creator Create(CreatorId creatorId, string creatorName, CreatorRole creatorRole)
        {
            if (_creatorBusinessRulesChecker.CreatorIdCanNotBeEmptyRuleIsBroken(creatorId))
                throw new InvalidCreatorIdException(creatorId);
            if (_creatorBusinessRulesChecker.CreatorNameCanNotBeEmptyRuleIsBroken(creatorName))
                throw new CreatorNameCanNotBeEmptyException();
            if (_creatorBusinessRulesChecker.CreatorNameCanNotBeShorterThanRuleIsBroken(creatorName))
                throw new CreatorNameIsTooShortException(GlobalValidationVariables.CreatorNameMinLength);
            if (_creatorBusinessRulesChecker.CreatorNameCanNotBeLongerThanRuleIsBroken(creatorName))
                throw new CreatorNameIsTooLongException(GlobalValidationVariables.CreatorNameMaxLength);

            var creator = new Creator(creatorId, creatorName, creatorRole);
            _domainEventEmitter.Emit(creator, new NewCreatorCreatedDomainEvent(creatorId, creatorName, creatorRole));
            return creator;
        }
    }
}