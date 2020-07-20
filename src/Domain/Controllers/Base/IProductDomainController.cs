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

using System.Collections.Generic;
using System.Threading.Tasks;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Types;
using SoftSentre.Shoppingendly.Services.Products.Domain.Aggregates;
using SoftSentre.Shoppingendly.Services.Products.Domain.Entities;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects.StronglyTypedIds;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.Controllers.Base
{
    public interface IProductDomainController
    {
        Task<Maybe<Product>> GetProductAsync(ProductId productId);
        Task<Maybe<Product>> GetProductWithCategoriesAsync(ProductId productId);
        Task<Maybe<IEnumerable<Categorization>>> GetAssignedCategoriesAsync(ProductId productId);

        Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string productName,
            Producer producer);

        Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string productName,
            Producer producer, Picture productPicture);

        Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string productName,
            Producer producer, IEnumerable<CategoryId> categoryIds);

        Task<Maybe<Product>> AddNewProductAsync(ProductId productId, CreatorId creatorId, string productName,
            Producer producer, Picture productPicture, IEnumerable<CategoryId> categoryIds);

        Task<bool> UploadProductPictureAsync(ProductId productId, Picture productPicture);
        Task<bool> ChangeProductNameAsync(ProductId productId, string productName);
        Task<bool> ChangeProductProducerAsync(ProductId productId, Producer productProducer);
        Task AssignProductToCategoryAsync(ProductId productId, CategoryId categoryId);
        Task DeallocateProductFromCategoryAsync(ProductId productId, CategoryId categoryId);
        Task DeallocateProductFromAllCategoriesAsync(ProductId productId);
        Task RemoveProductAsync(ProductId productId);
    }
}