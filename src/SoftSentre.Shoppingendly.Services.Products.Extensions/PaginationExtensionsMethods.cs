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
using System.Collections.Generic;
using System.Linq;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Queries;
using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Pagination.Results;

namespace SoftSentre.Shoppingendly.Services.Products.Extensions
{
    public static class PaginationExtensionsMethods
    {
        public static PagedResult<T> Paginate<T>(this IEnumerable<T> collection, PagedQueryBase query)
            => collection.Paginate(query.Page, query.Results);

        public static PagedResult<T> Paginate<T>(this IEnumerable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
                page = 1;

            if (resultsPerPage <= 0)
                resultsPerPage = 10;

            var enumerable = collection.ToList();
            
            if (enumerable.IsEmpty())
                return PagedResult<T>.Empty;

            var totalResults = enumerable.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var data = enumerable.Limit(page, resultsPerPage).ToList();

            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection, PagedQueryBase query)
            => collection.Limit(query.Page, query.Results);

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
                page = 1;

            if (resultsPerPage <= 0)
                resultsPerPage = 10;

            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }
    }
}