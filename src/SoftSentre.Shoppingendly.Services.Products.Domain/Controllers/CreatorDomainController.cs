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

using System.Threading.Tasks;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Globals;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Controllers
{
    public class CreatorDomainController : ICreatorDomainController
    {
        private readonly ICreatorBusinessRulesChecker _creatorBusinessRulesChecker;
        private readonly ICreatorRepository _creatorRepository;
        private readonly CreatorFactory _creatorFactory;

        public CreatorDomainController(ICreatorBusinessRulesChecker creatorBusinessRulesChecker,
            ICreatorRepository creatorRepository, CreatorFactory creatorFactory)
        {
            _creatorBusinessRulesChecker = creatorBusinessRulesChecker.IfEmptyThenThrowAndReturnValue();
            _creatorRepository = creatorRepository.IfEmptyThenThrowAndReturnValue();
            _creatorFactory = creatorFactory.IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Creator>> GetCreatorByIdAsync(CreatorId creatorId)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId);
            return creator;
        }

        public async Task<Maybe<Creator>> GetCreatorWithProductsByIdAsync(CreatorId creatorId)
        {
            var creator = await _creatorRepository.GetWithIncludesAsync(creatorId);
            return creator;
        }

        public async Task<Maybe<Creator>> AddNewCreatorAsync(CreatorId creatorId, string creatorName,
            CreatorRole creatorRole)
        {
            await _creatorRepository.GetByIdAndThrowIfEntityAlreadyExists(creatorId, new CreatorAlreadyExistsException(creatorId));

            var newCreator = _creatorFactory.Create(creatorId, creatorName, creatorRole);
            await _creatorRepository.AddAsync(newCreator);

            return newCreator;
        }

        public async Task ChangeCreatorNameAsync(CreatorId creatorId, string creatorName)
        {
            var creator =
                await _creatorRepository.GetByIdAndThrowIfEntityNotFound(creatorId,
                    new CreatorNotFoundException(creatorId));

            if (_creatorBusinessRulesChecker.CreatorNameCanNotBeEmptyRuleIsBroken(creatorName))
                throw new CreatorNameCanNotBeEmptyException();
            if (_creatorBusinessRulesChecker.CreatorNameCanNotBeShorterThanRuleIsBroken(creatorName))
                throw new CreatorNameIsTooShortException(GlobalValidationVariables.CreatorNameMinLength);
            if (_creatorBusinessRulesChecker.CreatorNameCanNotBeLongerThanRuleIsBroken(creatorName))
                throw new CreatorNameIsTooLongException(GlobalValidationVariables.CreatorNameMaxLength);

            creator.ChangeCreatorName(creatorName);
            _creatorRepository.Update(creator);
        }

        public async Task ChangeCreatorRoleAsync(CreatorId creatorId, CreatorRole creatorRole)
        {
            var creator =
                await _creatorRepository.GetByIdAndThrowIfEntityNotFound(creatorId,
                    new CreatorNotFoundException(creatorId));

            creator.ChangeCreatorRole(creatorRole);
            _creatorRepository.Update(creator);
        }
    }
}