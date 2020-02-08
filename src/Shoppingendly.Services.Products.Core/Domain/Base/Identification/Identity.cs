using System;

namespace Shoppingendly.Services.Products.Core.Domain.Base.Identification
{
    public abstract class Identity<TId> : IEquatable<Identity<TId>>, IIdentity<TId>
    {
        protected Identity()
        {
        }
        
        protected Identity(TId id)
        {
            Id = id;
        }

        public TId Id { get; protected set; }

        public bool Equals(Identity<TId> id)
        {
            if (ReferenceEquals(this, id)) return true;
            if (ReferenceEquals(null, id)) return false;
            return Id.Equals(id.Id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity<TId>);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}