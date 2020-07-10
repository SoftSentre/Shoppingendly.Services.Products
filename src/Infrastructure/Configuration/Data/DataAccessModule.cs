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

using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SoftSentre.Shoppingendly.Services.Products.Domain.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Extensions;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Repositories;
using SoftSentre.Shoppingendly.Services.Products.Infrastructure.EntityFramework.Settings;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Data
{
    public class DataAccessModule : Module
    {
        private readonly ILoggerFactory _loggerFactory;

        public DataAccessModule(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory
                .IfEmptyThenThrowOrReturnValue();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryEfRepository>()
                .As<ICategoryRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CreatorEfRepository>()
                .As<ICreatorRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductEfRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.Register(context =>
                {
                    var configuration = context.Resolve<IConfiguration>();
                    var options = configuration.GetOptions<SqlSettings>("sqlSettings");

                    return options;
                })
                .SingleInstance();

            builder.Register(context =>
                {
                    var sqlSettings = context.Resolve<SqlSettings>();
                    var dbContextOptions = new DbContextOptionsBuilder();

                    return new ProductServiceDbContext(_loggerFactory, sqlSettings,
                        dbContextOptions.Options);
                })
                .AsSelf()
                .As<IUnitOfWork>();
        }
    }
}