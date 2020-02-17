using System.Threading.Tasks;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class CreatorEfRepository :  ICreatorRepository
    {
        private readonly ProductServiceDbContext _productServiceDbContext;
        
        public CreatorEfRepository(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public Task<Maybe<Creator>> GetByIdAsync(CreatorId creatorId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<Creator>> GetByNameAsync(CreatorId creatorId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddAsync(Creator creator)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Creator creator)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Creator creator)
        {
            throw new System.NotImplementedException();
        }
    }
}