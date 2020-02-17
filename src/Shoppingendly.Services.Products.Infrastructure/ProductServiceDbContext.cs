using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Types;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;

namespace Shoppingendly.Services.Products.Infrastructure
{
    public class ProductServiceDbContext : DbContext
    {
        private Maybe<IDbContextTransaction> _currentTransaction;
        
        public const string DefaultSchema = "products";
        public bool HasActiveTransaction => _currentTransaction.HasValue;

        public DbSet<Product> Products { get; set; }    
        public DbSet<Category> Categories { get; set; }    
        public DbSet<Creator> Creators { get; set; }    
        public DbSet<ProductCategory> ProductCategories { get; set; }    
        public DbSet<Role> CreatorRoles { get; set; }
        
        protected ProductServiceDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new CreatorRolesConfiguration());
            modelBuilder.ApplyConfiguration(new CreatorsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
        }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction.Value;

        protected void SetDbContextTransaction(IDbContextTransaction transaction)
        {
            if (transaction == null) return;

            _currentTransaction = new Maybe<IDbContextTransaction>(transaction);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction.HasNoValue) return null;

            SetDbContextTransaction(await Database.BeginTransactionAsync());

            return _currentTransaction.Value;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction.Value)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction.HasValue)
                {
                    _currentTransaction.Value.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction.Value.Rollback();
            }
            finally
            {
                if (_currentTransaction.HasValue)
                {
                    _currentTransaction.Value.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}