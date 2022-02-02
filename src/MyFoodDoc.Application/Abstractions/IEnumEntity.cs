namespace MyFoodDoc.Application.Abstractions
{
    public interface IEnumEntity : IAuditableEntity
    {
        public string Key { get; set; }

        public string Name { get; set; }
    }
}
