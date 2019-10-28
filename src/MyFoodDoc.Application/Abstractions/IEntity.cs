namespace MyFoodDoc.Application.Abstractions
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
