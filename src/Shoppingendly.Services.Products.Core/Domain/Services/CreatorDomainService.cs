using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services.Creators;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services
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

        public async Task<Maybe<Creator>> AddNewCreatorAsync(CreatorId creatorId, string creatorName,
            string creatorEmail, Role creatorRole)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId);

            if (creator.HasValue)
            {
                throw new CreatorAlreadyExistsException(
                    $"Unable to add new creator, because creator with id: {creatorId} is already exists.");
            }

            var newCreator = Creator.Create(creatorId, creatorName, creatorEmail, creatorRole);
            await _creatorRepository.AddAsync(newCreator);

            return newCreator;
        }

        public async Task SetCreatorNameAsync(CreatorId creatorId, string creatorName)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId);
            var validatedCreator = IfCreatorIsEmptyThenThrow(creator);
            validatedCreator.SetName(creatorName);
            _creatorRepository.Update(validatedCreator);
        }

        public async Task SetCreatorEmailAsync(CreatorId creatorId, string creatorEmail)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId);
            var validatedCreator = IfCreatorIsEmptyThenThrow(creator);
            validatedCreator.SetEmail(creatorEmail);
            _creatorRepository.Update(validatedCreator);
        }

        public async Task SetCreatorRoleAsync(CreatorId creatorId, Role creatorRole)
        {
            var creator = await _creatorRepository.GetByIdAsync(creatorId);
            var validatedCreator = IfCreatorIsEmptyThenThrow(creator);
            validatedCreator.SetRole(creatorRole);
            _creatorRepository.Update(validatedCreator);
        }

        private static Creator IfCreatorIsEmptyThenThrow(Maybe<Creator> creator)
        {
            if (creator.HasNoValue)
            {
                throw new CreatorNotFoundException(
                    "Unable to mutate creator state, because value is empty.");
            }

            return creator.Value;
        }
    }
}