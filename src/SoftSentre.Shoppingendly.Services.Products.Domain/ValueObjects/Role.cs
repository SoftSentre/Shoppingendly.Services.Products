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
    public class Role : Enumeration
    {
        public static readonly Role Admin = new Role(1, "Admin");
        public static readonly Role Moderator = new Role(2, "Moderator");
        public static readonly Role User = new Role(3, "User");

        private Role(int id, string name) : base(id, name)
        {
        }
    }
}