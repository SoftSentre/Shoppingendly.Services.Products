using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Services.Base
{
    public interface ICreatorDomainService
    {
        Task<Maybe<Creator>> GetCreatorAsync(CreatorId creatorId);
        Task<Maybe<Creator>> GetCreatorByNameAsync(string name);
        Task<Maybe<Creator>> GetCreatorWithProductsAsync(string name);
        
        Task<Maybe<Creator>> AddNewCreatorAsync(CreatorId creatorId, 
            string creatorName, string creatorEmail, Role creatorRole);
        
        Task SetCreatorNameAsync(CreatorId creatorId, string creatorName);
        Task SetCreatorEmailAsync(CreatorId creatorId, string creatorEmail);
        Task SetCreatorRoleAsync(CreatorId creatorId, Role creatorRole);
    }
}