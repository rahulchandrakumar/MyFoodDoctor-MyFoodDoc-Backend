using Microsoft.AspNetCore.Http;

namespace MyFoodDoc.CMS.Payloads
{
    public class ImageUploadPayload
    {
        public IFormCollection Files { get; set; }
    }
}
