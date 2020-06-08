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

using SoftSentre.Shoppingendly.Services.Products.BasicTypes.Domain.ValueObjects;

namespace SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects
{
    public class CreatorRole : Enumeration
    {
        public static readonly CreatorRole Admin = new CreatorRole(1, "Admin");
        public static readonly CreatorRole Moderator = new CreatorRole(2, "Moderator");
        public static readonly CreatorRole User = new CreatorRole(3, "User");

        private CreatorRole(int id, string name) : base(id, name)
        {
        }
    }
}