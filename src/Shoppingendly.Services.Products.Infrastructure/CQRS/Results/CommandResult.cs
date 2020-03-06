using System.Collections.Generic;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Results
{
    public class CommandResult : Result, ICommandResult
    {
        public static CommandResult Success()
        {
            return new CommandResult {Ok = true};
        }

        public static CommandResult Failed(string error, ErrorType errorType)
        {
            return new CommandResult {Ok = false, ErrorType = errorType, Message = error};
        }

        public static CommandResult Failed(IDictionary<string, string> errors, ErrorType errorType)
        {
            return new CommandResult {Ok = false, ErrorType = errorType, Errors = errors};
        }
    }
}