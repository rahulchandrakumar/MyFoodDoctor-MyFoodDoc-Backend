namespace MyFoodDoc.CMS.Application.Models
{
    public class FileModel: BaseModel<int>
    {
        public byte[] Data { get; set; }
    }
}
