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

namespace SoftSentre.Shoppingendly.Services.Products.Application.CQRS.Base.Results
{
    public class QueryResult<T> : Result, IQueryResult<T>
    {
        public T Data { get; private set; }

        public static QueryResult<T> Success(T data)
        {
            return new QueryResult<T> {Ok = true, Data = data};
        }

        public static QueryResult<T> Failed(string error)
        {
            return new QueryResult<T> {Ok = false, Message = error};
        }

        public static QueryResult<T> Failed(IDictionary<string, string> errors)
        {
            return new QueryResult<T> {Ok = false, Errors = errors};
        }
    }
}