﻿// Copyright 2020 SoftSentre Contributors
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

namespace SoftSentre.Shoppingendly.Services.Products.BasicTypes.CQRS.Results
{
    public class CommandResult : CommonResult, ICommandResult
    {
        public static CommandResult Success()
        {
            return new CommandResult {Ok = true};
        }

        public static CommandResult Failed(string error)
        {
            return new CommandResult {Ok = false, Message = error};
        }

        public static CommandResult Failed(IDictionary<string, string> errors)
        {
            return new CommandResult {Ok = false, Errors = errors};
        }
    }
}