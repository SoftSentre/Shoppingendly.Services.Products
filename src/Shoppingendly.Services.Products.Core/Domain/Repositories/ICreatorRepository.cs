using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Core.Domain.Repositories
{
    public interface ICreatorRepository 
    {
        Task<Maybe<Creator>> GetByIdAsync(CreatorId creatorId);
        Task<Maybe<Creator>> GetByNameAsync(CreatorId creatorId);
        Task<bool> AddAsync(Creator creator);
        void Update(Creator creator);
        void Delete(Creator creator);
    }
}