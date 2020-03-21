﻿using System.Collections.Generic;

namespace Shoppingendly.Services.Products.Infrastructure.CQRS.Results
{
    public interface IResult
    {
        bool Ok { get; }

        string Message { get; }

        IDictionary<string, string> Errors { get; }
    }
}