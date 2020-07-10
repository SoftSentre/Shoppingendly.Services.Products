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

namespace SoftSentre.Shoppingendly.Services.Products.WebApi.Environments
{
    public class EnvironmentType : Enumeration
    {
        public static EnvironmentType Production = new EnvironmentType(1, "Production");
        public static EnvironmentType PreProduction = new EnvironmentType(2, "PreProduction");
        public static EnvironmentType Test = new EnvironmentType(3, "Test");
        public static EnvironmentType Development = new EnvironmentType(4, "Development");
        public static EnvironmentType Docker = new EnvironmentType(5, "Docker");
        
        public EnvironmentType(int id, string name) : base(id, name)
        {
        }
    }
}