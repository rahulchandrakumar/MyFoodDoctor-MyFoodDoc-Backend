namespace MyFoodDoc.Application.Abstractions
{
    public abstract class AbstractEntity : IEntity
    {
        public virtual int Id { get; set; }
    }
}
