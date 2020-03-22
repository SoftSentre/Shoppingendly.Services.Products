using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.Repositories;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories
{
    public class CreatorEfRepository : ICreatorRepository
    {
        private readonly ProductServiceDbContext _productServiceDbContext;

        public CreatorEfRepository(ProductServiceDbContext productServiceDbContext)
        {
            _productServiceDbContext = productServiceDbContext
                .IfEmptyThenThrowAndReturnValue();
        }

        public async Task<Maybe<Creator>> GetByIdAsync(CreatorId creatorId)
        {
            return await _productServiceDbContext.Creators.FirstOrDefaultAsync(p => p.Id.Equals(creatorId));
        }

        public async Task<Maybe<Creator>> GetByNameAsync(string name)
        {
            return await _productServiceDbContext.Creators.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Maybe<Creator>> GetWithIncludesAsync(string name)
        {
            return await _productServiceDbContext.Creators.Include(c => c.Products)
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task AddAsync(Creator creator)
        {
            await _productServiceDbContext.AddAsync(creator);
        }

        public void Update(Creator creator)
        {
            _productServiceDbContext.Creators.Update(creator);
        }

        public void Delete(Creator creator)
        {
            _productServiceDbContext.Creators.Remove(creator);
        }
    }
}