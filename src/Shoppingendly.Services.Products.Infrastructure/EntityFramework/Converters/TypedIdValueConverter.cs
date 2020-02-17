using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shoppingendly.Services.Products.Core.Domain.Base.Identification;

namespace Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters
{
    public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
        where TTypedIdValue : Identity<Guid>
    {
        public TypedIdValueConverter(ConverterMappingHints mappingHints = null)
            : base(id => id.Id, value => Create(value), mappingHints)
        {
        }

        private static TTypedIdValue Create(Guid id) =>
            Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue;
    }
}