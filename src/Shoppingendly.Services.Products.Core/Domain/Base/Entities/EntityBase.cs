using System;
using Shoppingendly.Services.Products.Core.Domain.Base.BusinessRules;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public abstract class EntityBase<TId> : IEntity<TId>
    {
        public TId Id { get; }

        protected EntityBase()
        {
        }
        
        protected EntityBase(TId id)
        {
            Id = id;
        }

        protected void CheckRule(IBusinessRule rule, Action throwProperException)
        {
            if (rule.IsBroken())
            {
                throwProperException.Invoke();
            }
        }
    }
}