namespace MyFoodDoc.CMS.Application.Models
{
    public class ImageModel: BaseModel
    {
        /// <summary>
        /// Only for retreiving
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Only for setting
        /// </summary>
        public byte[] ImageData { get; set; }
    }
}
