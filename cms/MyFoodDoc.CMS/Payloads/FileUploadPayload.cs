using Microsoft.AspNetCore.Http;

namespace MyFoodDoc.CMS.Payloads
{
    public class FileUploadPayload
    {
        public IFormCollection Files { get; set; }
    }
}
