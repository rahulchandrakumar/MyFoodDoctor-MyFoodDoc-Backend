using MyFoodDoc.CMS.Application.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IImageService
    {
        Task<ImageModel> UploadImage(Stream stream, CancellationToken cancellationToken = default);
    }
}
