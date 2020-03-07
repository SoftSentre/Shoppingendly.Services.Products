using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shoppingendly.Services.Products.Core.Domain.Aggregates;
using Shoppingendly.Services.Products.Core.Domain.Entities;
using Shoppingendly.Services.Products.Core.Domain.ValueObjects;
using Shoppingendly.Services.Products.Core.Extensions;
using Shoppingendly.Services.Products.Core.Types;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Extensions;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework
{
    public class ProductServiceDbContext : DbContext, IUnitOfWork
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly SqlSettings _sqlSettings;
        private Maybe<IDbContextTransaction> _currentTransaction;

        public const string DefaultSchema = "products";
        public bool HasActiveTransaction => _currentTransaction.HasValue;

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Role> CreatorRoles { get; set; }

        public ProductServiceDbContext(SqlSettings sqlSettings, ILoggerFactory loggerFactory,
            DbContextOptions options) : base(options)
        {
            _sqlSettings = sqlSettings
                .IfEmptyThenThrowAndReturnValue();
            
            _loggerFactory = loggerFactory
                .IfEmptyThenThrowAndReturnValue();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            if (_sqlSettings.UseInMemory)
            {
                optionsBuilder.UseInMemoryDatabase(_sqlSettings.Database)
                    .UseStronglyTypedIds()
                    .UseLogging(_loggerFactory);
                    
                return;
            }

            optionsBuilder.UseSqlServer(_sqlSettings.ConnectionString)
                .UseStronglyTypedIds()
                .UseLogging(_loggerFactory);
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

        public async Task<bool> SaveAsync()
        {
            var numberOfRows = await SaveChangesAsync();

            return numberOfRows > 0;
        }

        private void UseStronglyTypedIds(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        }
    }
}