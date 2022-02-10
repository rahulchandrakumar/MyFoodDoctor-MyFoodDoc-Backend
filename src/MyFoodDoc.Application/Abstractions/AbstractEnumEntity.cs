namespace MyFoodDoc.Application.Abstractions
{
    public abstract class AbstractEnumEntity : AbstractAuditableEntity, IEnumEntity
    {
        public virtual string Key { get; set; }

        public virtual string Name { get; set; }
    }
}
