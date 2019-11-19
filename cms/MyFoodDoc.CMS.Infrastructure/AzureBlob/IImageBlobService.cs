using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.AzureBlob
{
    public interface IImageBlobService
    {
        Task<bool> DeleteImage(string url);
        Task<string> UploadImage(byte[] data, string fileType, string filename = null);
    }
}