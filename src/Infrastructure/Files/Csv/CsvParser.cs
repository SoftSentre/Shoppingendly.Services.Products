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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using SoftSentre.Shoppingendly.Services.Products.Domain.ValueObjects;
using SoftSentre.Shoppingendly.Services.Products.Extensions;

namespace SoftSentre.Shoppingendly.Services.Products.Infrastructure.Files.Csv
{
    public class CsvParser : ICsvParser
    {
        private readonly string _contentRoot;

        private readonly string _setupContentRoot = "Setup";
        
        private readonly string _categoriesFilename = "Categories.csv"; 
        private readonly string _creatorFilename = "CreatorRoles.csv"; 
        private readonly string _creatorRolesFilename = "Creators.csv"; 
        private readonly string _productCategoryFilename = "ProductCategory.csv"; 
        private readonly string _productsFilename = "Products.csv"; 
        
        public CsvParser(string contentRoot)
        {
            _contentRoot = contentRoot.IfEmptyThenThrowOrReturnValue();
        }

        public IEnumerable<CreatorRole> LoadCreatorRoles()
        {
            var creatorRolesFilePath = Path.Combine(_contentRoot, _setupContentRoot, _creatorFilename);
            var streamReader = new StreamReader(creatorRolesFilePath);
            var reader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            
            reader.Configuration.HasHeaderRecord = true;
            reader.Configuration.Delimiter = ",";     

            return reader.GetRecords<CreatorRole>().ToList();
        }
    }
    
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}