namespace Shoppingendly.Services.Products.Core.Domain.Base.Entities
{
    public class DoubleKeyEntityBase<TFirstId, TSecondId> : IDoubleKeyEntity<TFirstId, TSecondId>
    {
        public TFirstId FirstKey { get; }
        public TSecondId SecondKey { get; }

        protected DoubleKeyEntityBase()
        {
        }

        protected DoubleKeyEntityBase(TFirstId firstKey, TSecondId secondKey)
        {
            FirstKey = firstKey;
            SecondKey = secondKey;
        }
    }
}