using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services.Base
{
    public interface ICreatorDomainService
    {
        Maybe<Creator> AddNewCreator(CreatorId creatorId, 
            string creatorName, string creatorEmail, Role creatorRole);
        
        void SetCreatorName(Maybe<Creator> creator, string creatorName);
        void SetCreatorEmail(Maybe<Creator> creator, string creatorEmail);
        void SetCreatorRole(Maybe<Creator> creator, Role creatorRole);
    }
}