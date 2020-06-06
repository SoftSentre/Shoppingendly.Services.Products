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
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.Exceptions.Services.Creators;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Services
{
    public class CreatorDomainService : ICreatorDomainService
    {
        private readonly ICreatorRepository _creatorRepository;

        public CreatorDomainService(ICreatorRepository creatorRepository)
        {
            _creatorRepository = creatorRepository
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Creator>> GetCreatorAsync(CreatorId creatorId)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId);
            return creator;
        }

        public async Task<Maybe<Creator>> GetCreatorByNameAsync(string name)
        {
            var creator = await _creatorRepository.GetByNameAsync(name);
            return creator;
        }

        public async Task<Maybe<Creator>> GetCreatorWithProductsAsync(string name)
        {
            var creator = await _creatorRepository.GetWithIncludesAsync(name);
            return creator;
        }

        public async Task<Maybe<Creator>> AddNewCreatorAsync(CreatorId creatorId, string creatorName, Role creatorRole)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId);

            if (creator.HasValue)
            {
                throw new CreatorAlreadyExistsException(
                    $"Unable to add new creator, because creator with id: {creatorId} is already exists.");
            }

            var newCreator = Creator.Create(creatorId, creatorName, creatorRole);
            await _creatorRepository.AddAsync(newCreator);

            return newCreator;
        }

        public async Task SetCreatorNameAsync(CreatorId creatorId, string creatorName)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId).UnwrapAsync(
                new CreatorNotFoundException(
                    $"Unable to mutate creator state, because creator with id: {creatorId} not found."));

            creator.SetName(creatorName);
            _creatorRepository.Update(creator);
        }

        public async Task SetCreatorRoleAsync(CreatorId creatorId, Role creatorRole)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId).UnwrapAsync(
                new CreatorNotFoundException(
                    $"Unable to mutate creator state, because creator with id: {creatorId} not found."));

            creator.SetRole(creatorRole);
            _creatorRepository.Update(creator);
        }
    }
}