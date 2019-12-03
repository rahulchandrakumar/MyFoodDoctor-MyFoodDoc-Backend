namespace MyFoodDoc.CMS.Models.VMBase
{
    public class ColabDataTableBaseModel<T> : BaseModel<T>
    {
        public string Editor { get; set; }
        public long? LockDate { get; set; }
    }
}
