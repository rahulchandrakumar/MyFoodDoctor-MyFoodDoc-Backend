using MyFoodDoc.CMS.Application.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Application.Persistence
{
    public interface IImageService: IServiceBase<ImageModel>
    {
        Task<ImageModel> UploadImage(byte[] data, CancellationToken cancellationToken = default);
    }
}
