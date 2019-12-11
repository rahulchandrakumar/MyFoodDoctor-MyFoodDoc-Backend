using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.AzureBlob
{
    public interface IImageBlobService
    {
        Task<bool> DeleteImage(string url, CancellationToken cancellationToken = default);
        Task<string> UploadImage(Stream stream, string fileType, string filename = null, CancellationToken cancellationToken = default);
    }
}