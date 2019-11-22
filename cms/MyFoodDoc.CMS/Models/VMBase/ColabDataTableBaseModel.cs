namespace MyFoodDoc.CMS.Models.VMBase
{
    public class ColabDataTableBaseModel<T>
    {
        public T Id { get; set; }
        public string Editor { get; set; }
        public long? LockDate { get; set; }
    }
}
