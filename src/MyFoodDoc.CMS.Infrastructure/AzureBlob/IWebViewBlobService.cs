using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.AzureBlob
{
    public interface IWebViewBlobService
    {
        Task<bool> UpdateFile(string content, string url, CancellationToken cancellationToken = default);
    }
}