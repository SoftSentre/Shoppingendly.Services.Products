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
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers;
using SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base;
using SoftSentre.Shoppingendly.Services.Products.Domain.Factories;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services;
using SoftSentre.Shoppingendly.Services.Products.Domain.Services.Base;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Configuration.Domain
{
    public class DomainModule : Module
    {
        private AllConstructorFinder _allConstructorFinder;
        
        protected override void Load(ContainerBuilder builder)
        {
            _allConstructorFinder = new AllConstructorFinder();
            
            builder.RegisterType<ProductDomainController>()
                .As<IProductDomainController>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CategoryDomainController>()
                .As<ICategoryDomainController>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CreatorDomainController>()
                .As<ICreatorDomainController>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CategoryBusinessRulesChecker>()
                .As<ICategoryBusinessRulesChecker>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<CreatorBusinessRulesChecker>()
                .As<ICreatorBusinessRulesChecker>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<ProductBusinessRulesChecker>()
                .As<IProductBusinessRulesChecker>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<DomainEventsEmitter>()
                .As<IDomainEventEmitter>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<DomainEventsManager>()
                .As<IDomainEventsManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CreatorFactory>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(_allConstructorFinder);
            
            builder.RegisterType<CategoryFactory>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(_allConstructorFinder);
            
            builder.RegisterType<ProductFactory>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(_allConstructorFinder);
        }
    }
}