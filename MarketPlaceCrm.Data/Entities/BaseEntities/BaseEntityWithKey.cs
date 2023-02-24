namespace MarketPlaceCrm.Data.Entities
{
    public abstract class BaseEntityWithKey<TKey>
    {
        public TKey Id { get; set; }
    }
}