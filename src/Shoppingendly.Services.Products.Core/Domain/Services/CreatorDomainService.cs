using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Services.Base;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Exceptions.Services;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services
{
    public class CreatorDomainService : ICreatorDomainService
    {
        public Maybe<Creator> AddNewCreator(CreatorId creatorId, string creatorName, string creatorEmail, Role creatorRole)
        {
            var creator = Creator.Create(creatorId, creatorName, creatorEmail, creatorRole);

            return creator;
        }

        public void SetCreatorName(Maybe<Creator> creator, string creatorName)
        {
            var validatedCreator = IfCreatorIsEmptyThenThrow(creator);
            validatedCreator.SetName(creatorName);
        }

        public void SetCreatorEmail(Maybe<Creator> creator, string creatorEmail)
        {
            var validatedCreator = IfCreatorIsEmptyThenThrow(creator);
            validatedCreator.SetEmail(creatorEmail);
        }

        public void SetCreatorRole(Maybe<Creator> creator, Role creatorRole)
        {
            var validatedCreator = IfCreatorIsEmptyThenThrow(creator);
            validatedCreator.SetRole(creatorRole);
        }

        private static Creator IfCreatorIsEmptyThenThrow(Maybe<Creator> creator)
        {
            if (creator.HasNoValue)
            {
                throw new EmptyCreatorProvidedException(
                    "Unable to mutate creator state, because provided value is empty.");
            }

            return creator.Value;
        }
    }
}