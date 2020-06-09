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

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.DomainEvents;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.EntityTypeConfigurations;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework
{
    public class ProductServiceDbContext : DbContext, IUnitOfWork
    {
        public const string DefaultSchema = "products";
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        private readonly ILoggerFactory _loggerFactory;
        private readonly SqlSettings _sqlSettings;

        private Maybe<IDbContextTransaction> _currentTransaction;

        public ProductServiceDbContext(ILoggerFactory loggerFactory,
            IDomainEventsDispatcher domainEventsDispatcher, SqlSettings sqlSettings,
            DbContextOptions options) : base(options)
        {
            _domainEventsDispatcher = domainEventsDispatcher
                .IfEmptyThenThrowAndReturnValue();

            _sqlSettings = sqlSettings
                .IfEmptyThenThrowAndReturnValue();

            _loggerFactory = loggerFactory
                .IfEmptyThenThrowAndReturnValue();
        }

        public bool HasActiveTransaction => _currentTransaction.HasValue;

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<CreatorRole> CreatorRoles { get; set; }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return _currentTransaction.Value;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction.HasNoValue)
            {
                return null;
            }

            SetDbContextTransaction(await Database.BeginTransactionAsync());

            return _currentTransaction.Value;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != _currentTransaction.Value)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                await _domainEventsDispatcher.DispatchAsync();
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

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

        private void SetDbContextTransaction(IDbContextTransaction transaction)
        {
            if (transaction == null)
            {
                return;
            }

            _currentTransaction = new Maybe<IDbContextTransaction>(transaction);
        }

        private void UseStronglyTypedIds(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        }
    }
}