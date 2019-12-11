namespace MyFoodDoc.CMS.Models.VMBase
{
    public class EditStateHubModel: BaseModel<int>
    {
        public string Editor { get; set; }
        public long? LockDate { get; set; }
    }
}
