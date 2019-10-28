namespace MyFoodDoc.Application.Abstractions
{
    public abstract class AbstractEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
